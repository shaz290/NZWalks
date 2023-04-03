using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Data;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
            var walkDifficultyDomain = await walkDifficultyRepository.GetAllAsync();

            //Convert Domain to DTOs

            var walkDifficultyDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficultyDomain);

            return Ok(walkDifficultyDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyById")]
        [Authorize(Roles = "reader")]


        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetWalkDifficultyByIdAsync(id);

            if (walkDifficulty == null)
            {
                return NotFound();
            }
            //Convert Domain to DTOs

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //if(!ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest))
            //{
            //    return BadRequest(ModelState);
            //}
            //Convert DTO to Domain
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
            };
            //Call Repository

            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            //Convert Domain to DTO

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            return CreatedAtAction(nameof(GetWalkDifficultyById), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);


        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync(Guid id, Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //if(!ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest))
            //{
            //    return BadRequest(ModelState);
            //}
            //Convert DTO to Domain

            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };

            //Call Repository to update
            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);

            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            return Ok(walkDifficultyDTO);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            var walkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);

            if (walkDifficultyDomain == null)
            {
                return NotFound();
            }

            //Convert to DTO

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            return Ok(walkDifficultyDTO);
        }

        #region private methods
        private bool ValidateAddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {

            if (addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest), $"Add Region Data is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code), $"{nameof(addWalkDifficultyRequest.Code)} cannot be null EMpty or White space.");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateUpdateWalkDifficultyAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest), $"Add Region Data is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code), $"{nameof(updateWalkDifficultyRequest.Code)} cannot be null EMpty or White space.");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
