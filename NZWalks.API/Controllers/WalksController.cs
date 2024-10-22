﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper,IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        // CREATE Walk
        // [POST]:  https://localhost:portnumber/api/walks
        [HttpPost]
        [ValidateModel] // Custom action attribute
        public async Task<IActionResult> Create (AddWalkRequestDto addWalkRequestDto)
        {
            //Map Dto to Domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);

            await walkRepository.CreateAsync(walkDomainModel);

            //Map Domain model to Dto
            var walkDto = mapper.Map<WalkDto>(walkDomainModel);

            return Ok(walkDto);
        }

        // GET: Walks
        // GET: /api/walks?filterOn={filterOn}&filterQuery={filterQuery}&sortBy={sortBy}&isAscending={isAscending}
        //&pageNumber={pageNumber}&pageSize={pageSize}
        [MapToApiVersion("1.0")]
        [HttpGet]
        public async Task<IActionResult> GetAllV1([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var walkDomainModel = await walkRepository.GetAllAsync(filterOn,filterQuery,sortBy, isAscending?? true
                ,pageNumber,pageSize);

            //Map Domain to Dto
            var mapDto = mapper.Map<List<WalkDto>>(walkDomainModel);

            return Ok(mapDto);
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        public async Task<IActionResult> GetAllV2([FromQuery] string? filterOn, [FromQuery] string? filterQuery,[FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var walkDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true
                , pageNumber, pageSize);

            //Map Domain to Dto
            var mapDto = mapper.Map<List<WalkDto>>(walkDomainModel);

            return Ok("v2");
        }

        // GET Walk by Id
        // GET: /api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetById(id);

            if(walkDomainModel == null)
                return NotFound();

            // Map domain to dto
            var walkDto = mapper.Map<WalkDto>(walkDomainModel);

            return Ok(walkDto); 
        }

        //Update Walk by Id
        //PUT: /api/Walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel] // Custom action attribute
        public async Task <IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
             
            // Map Dto to domain model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);

            await walkRepository.UpdateAsync(id,walkDomainModel);

            if (walkDomainModel == null)
                return NotFound();

            // map model to dto
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        // Delete a walk by id
        // DELETE: /api/Walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);

            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }

            // Map domain model to dto
            return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));
        }
    }
}
