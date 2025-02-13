﻿ using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepositories regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext, 
            IRegionRepositories regionRepository,
            IMapper mapper,
            ILogger<RegionsController> logger) {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET ALL REGIONS
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        //[Authorize(Roles ="Reader,Writer")]
        public async Task<IActionResult> GetAll()
        {
            logger.LogInformation(" Get All Regions Action method was invoked");

            // Get Data From Database - Domain Models
            var regions = await regionRepository.GetAllAsync();

            logger.LogInformation($"Finished Get All Async:{JsonSerializer.Serialize(regions)}");

            // Map Domain Models to DTOs
            var regionsDto = mapper.Map<List<RegionDto>>(regions);

            //Return DTOs
            return Ok(regionsDto);
        }

        // GET SINGLE REGIONS
        // GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        //[Authorize(Roles = "Reader,Writer")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetbyId([FromRoute] Guid id)
        {
            // Get Data From Database - Domain Models
            var region = await regionRepository.GetByIdAsync(id);  

            if(region == null)
            {
                return NotFound();
            }

            // Map Domain Models to DTOs
            var regionDto = mapper.Map<RegionDto>(region);

            //Return DTOs
            return Ok(regionDto);
        }

        // POST: Create a new region
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        //[Authorize(Roles = "Writer")]
        [ValidateModel] // Custom action attribute
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {

            // Map or Convert DTO to Domain Model
            var regionDomainModel  =  mapper.Map<Region>(addRegionRequestDto);

            // Use Domain Model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map Domain Model to Dto
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetbyId),new {id = regionDto.Id},regionDto);
        }

        //Update region
        //PUT: https://localhost:portnumber/api/regions
        [HttpPut]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        [ValidateModel] // Custom action attribute
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {

            // Map Dto to domain model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if(regionDomainModel == null)
                return NotFound();

            // Convert Domain to Dto

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }

        //Delete region
        //DELETE: https://localhost:portnumber/api/regions
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if(regionDomainModel == null) return NotFound();

            // return deleted region back
            var regionDto = mapper.Map<List<RegionDto>>(regionDomainModel);

            return Ok(regionDto);
        }
    }
}
