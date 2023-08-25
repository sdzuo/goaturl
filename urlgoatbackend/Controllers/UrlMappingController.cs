using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using urlgoatbackend.Dto;
using urlgoatbackend.Interfaces;
using urlgoatbackend.Models;
using urlgoatbackend.Helper;
using Microsoft.AspNetCore.Cors; // Enable CORS

namespace urlgoatbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlMappingController : ControllerBase
    {
        private readonly IUrlMappingRepository _urlMappingRepository;
        private readonly IMapper _mapper;

        // Constructor with dependency injection
        public UrlMappingController(IUrlMappingRepository urlMappingRepository, IMapper mapper)
        {
            _urlMappingRepository = urlMappingRepository;
            _mapper = mapper;
        }

        [EnableCors] // Enable CORS for this controller
        [HttpPost("CreateShortUrl")] // Route for creating short URLs
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateShortUrl([FromBody] CreateShortUrlDto shortUrlCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Return validation errors if model state is invalid

            // Check if the long URL already exists in the database
            var existingUrlMap = await _urlMappingRepository.GetUrlMappingByUrl(shortUrlCreate.LongUrl);

            if (existingUrlMap == null)
            {
                // Generate a short key for the new URL
                string shortKey = HashingHelper.HashLongUrl(shortUrlCreate.LongUrl);

                // Map the DTO to a UrlMapping entity
                var urlMap = _mapper.Map<CreateShortUrlDto, UrlMapping>(shortUrlCreate);
                urlMap.ShortKey = shortKey;

                // Create the short URL mapping and save it to the database
                _urlMappingRepository.CreateShortUrl(urlMap);
                await _urlMappingRepository.SaveAsync();

                // Return a response with the newly created short URL
                var response = new
                {
                    shortenedUrl = $"http://localhost:5150/api/UrlMapping/{shortKey}",
                    sKey = shortKey,
                    newurl = true
                };

                return Ok(response); // Return a 200 OK response
            }

            // If the long URL already exists, return the existing short URL
            var existingResponse = new
            {
                shortenedUrl = $"http://localhost:5150/api/UrlMapping/{existingUrlMap.ShortKey}",
                sKey = existingUrlMap.ShortKey,
                newurl = false
            };

            return Ok(existingResponse); // Return a 200 OK response
        }

        [EnableCors] // Enable CORS for this controller
        [HttpGet("{shortKey}")] // Route for redirecting to the original URL
        [ProducesResponseType(302)] // Redirect status code
        [ProducesResponseType(404)] // Not found status code
        public async Task<IActionResult> RedirectToOriginalUrl(string shortKey)
        {
            // Retrieve the original URL mapping based on the short key
            UrlMapping urlMapping = await _urlMappingRepository.GetOriginalUrlByShortKeyAsync(shortKey);

            if (urlMapping == null)
            {
                return NotFound("Shortened URL not found"); // Return a 404 response if the short URL is not found
            }

            // Redirect to the original URL
            return Redirect(urlMapping.LongUrl);
        }
    }
}
