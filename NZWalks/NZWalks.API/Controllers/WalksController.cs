using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //Fetch Data from DB

            var walksDomain = await walkRepository.GetAllAsync();
            //Convert domain walks to DTO walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            //Return response

            return Ok(walksDTO);

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalksAsync")]
        public async Task<IActionResult> GetWalksAsync(Guid id)
        {
            //Get walk domain object from DB
            var walkDomain = await walkRepository.GetByIdAsync(id);

            //Convert Domain object to DTO

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            //Return Response

            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Convert DTO to Domain object
            var walkDomain = new Models.Domain.Walk
            {
                Length = addWalkRequest.Length,
                Name = addWalkRequest.Name,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            //Pass domain object to Repository to Persisit this
            walkDomain = await walkRepository.AddAsync(walkDomain);
            //Convert the Domain object back to DTO

            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            //Send DTO response back to client
            return CreatedAtAction(nameof(GetWalksAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Convert DTO to Domain object
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            //Pass Detials to Repository - Get Domain object in response (or null)
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
            //Handle Null(Not Found)

            if (walkDomain == null)
            {
                return NotFound();
            }
            else
            {
                //Convert back Domain to DTO
                var walkDTO = new Models.DTO.Walk
                {
                    Id = walkDomain.Id,
                    Length = walkDomain.Length,
                    Name = walkDomain.Name,
                    RegionId = walkDomain.RegionId,
                    WalkDifficultyId = walkDomain.WalkDifficultyId
                };


                //Retrun Response
                return Ok(walkDTO);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //Call Repository to delete walk

            var walkDOmain = await walkRepository.DeleteAsync(id);
            if (walkDOmain == null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDOmain);
            return Ok(walkDTO);
        }
    }
}
