using Microsoft.AspNetCore.Mvc;
using restaurant_api.Model;
using restaurant_api.Services.Interface;

namespace restaurant_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPost]
        public async Task<IActionResult> AddRestaurant([FromBody] Restaurant restaurant)
        {
            restaurant.Id = Guid.NewGuid().ToString();
            await _restaurantService.AddUpdateRestaurantAsync(restaurant);
            return Ok(new ApiResponse()
            {
                Message = "Restaurant created successfully."
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurant(string id)
        {
            var restaurant = await _restaurantService.GetRestaurantAsync(id);
            if (restaurant == null)
                return NotFound(new ApiResponse()
                {
                    Message = "Invalid restaurant id."
                });

            return Ok(new ApiResponse()
            {
                Message = "Restaurant data fetched successfully.",
                Data = restaurant
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRestaurants()
        {
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();
            return Ok(new ApiResponse()
            {
                Message = "Restaurant data fetched successfully.",
                Data = restaurants
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRestaurant(string id, [FromBody] Restaurant updatedRestaurant)
        {
            var restaurant = await _restaurantService.GetRestaurantAsync(id);
            if (restaurant == null)
                return NotFound(new ApiResponse()
                {
                    Message = "Invalid restaurant id."
                });
            restaurant = updatedRestaurant;
            restaurant.Id = id;

            await _restaurantService.AddUpdateRestaurantAsync(restaurant);
            return Ok(new ApiResponse()
            {
                Message = "Restaurant created successfully."
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant(string id)
        {
            var restaurant = await _restaurantService.GetRestaurantAsync(id);
            if (restaurant == null)
                return NotFound(new ApiResponse()
                {
                    Message = "Invalid restaurant id."
                });
            await _restaurantService.DeleteRestaurant(restaurant);
            return Ok(new ApiResponse()
            {
                Message = "Restaurant deleted successfully."
            });
        }

        [HttpPut("UpdateRestaurantRatings/{id}")]
        public async Task<IActionResult> UpdateRestaurantRatings(string id, [FromBody] int rating)
        {
            var restaurant = await _restaurantService.GetRestaurantAsync(id);
            if (restaurant == null)
                return NotFound(new ApiResponse()
                {
                    Message = "Invalid restaurant id."
                });

            await _restaurantService.UpdateRestaurantRatings(restaurant, rating);
            return Ok(new ApiResponse()
            {
                Message = "Rating updated successfully."
            });
        }

    }
}
