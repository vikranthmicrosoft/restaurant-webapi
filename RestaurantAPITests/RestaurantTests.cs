using Microsoft.AspNetCore.Mvc;
using Moq;
using restaurant_api.Controllers;
using restaurant_api.Model;
using restaurant_api.Services.Interface;
using Xunit;

namespace RestaurantAPITests
{
    public class RestaurantTests
    {
        private readonly RestaurantController _controller;
        private readonly Mock<IRestaurantService> _restaurantServiceMock = new Mock<IRestaurantService>();
        public RestaurantTests()
        {
            _controller = new RestaurantController(_restaurantServiceMock.Object);
        }

        #region AddUpdateRestaurantAsync
        [Fact]
        public async Task AddRestaurant_ReturnsOkResult()
        {
            // Arrange
            var restaurant = new Restaurant()
            {
                Id = Guid.NewGuid().ToString(),
                Address = "Test",
                AverageRating = 0,
                Description = "Test Desc",
                Hours = "1",
                Name = "Balaji",
                Ratings = new List<NumberList>() { new NumberList() { Number = 1 } }
            };

            // Act
            var result = await _controller.AddRestaurant(restaurant);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal("Restaurant created successfully.", apiResponse.Message);
        }

        [Fact]
        public async Task AddRestaurant_SetsRestaurantId()
        {
            // Arrange
            var restaurant = new Restaurant()
            {
                Id = Guid.NewGuid().ToString(),
                Address = "Test",
                AverageRating = 0,
                Description = "Test Desc",
                Hours = "1",
                Name = "Balaji",
                Ratings = new List<NumberList>() { new NumberList() { Number = 1 } }
            };

            // Act
            await _controller.AddRestaurant(restaurant);

            // Assert
            Assert.NotNull(restaurant.Id);
            Assert.NotEmpty(restaurant.Id);
            Assert.True(Guid.TryParse(restaurant.Id, out _));
        }
        #endregion

        #region GetRestaurant
        [Fact]
        public void GetRestaurant_ReturnsOkResultWithData()
        {
            // Arrange
            var restaurantId = Guid.NewGuid().ToString();
            var restaurant = new Restaurant { /* Initialize restaurant properties */ };
            _restaurantServiceMock.Setup(x => x.GetRestaurantAsync(restaurantId)).ReturnsAsync(restaurant);

            // Act
            var result = _controller.GetRestaurant(restaurantId).Result;

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal("Restaurant data fetched successfully.", apiResponse.Message);
            Assert.Equal(restaurant, apiResponse.Data);
        }

        [Fact]
        public void GetRestaurant_ReturnsNotFoundResult()
        {
            // Arrange
            var invalidRestaurantId = "invalidId";
            _restaurantServiceMock.Setup(x => x.GetRestaurantAsync(invalidRestaurantId)).ReturnsAsync((Restaurant)null);

            // Act
            var result = _controller.GetRestaurant(invalidRestaurantId).Result;

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(notFoundResult.Value);
            Assert.Equal("Invalid restaurant id.", apiResponse.Message);
        }
        #endregion

        #region GetAllRestaurants
        [Fact]
        public async Task GetAllRestaurants_ReturnsOkResultWithData()
        {
            // Arrange
            var restaurants = new List<Restaurant>
            {
                new Restaurant { /* Initialize restaurant properties */ },
                new Restaurant { /* Initialize another restaurant properties */ }
            };
            _restaurantServiceMock.Setup(x => x.GetAllRestaurantsAsync()).ReturnsAsync(restaurants);

            // Act
            var result = await _controller.GetAllRestaurants();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal("Restaurant data fetched successfully.", apiResponse.Message);
            Assert.Equal(restaurants, apiResponse.Data);
        }
        #endregion

        #region UpdateRestaurant
        [Fact]
        public async Task UpdateRestaurant_ReturnsOkResult()
        {
            // Arrange
            var restaurantId = Guid.NewGuid().ToString();
            var updatedRestaurant = new Restaurant { /* Initialize updated restaurant properties */ };
            var existingRestaurant = new Restaurant { Id = restaurantId, /* Initialize existing restaurant properties */ };

            _restaurantServiceMock.Setup(x => x.GetRestaurantAsync(restaurantId)).ReturnsAsync(existingRestaurant);

            // Act
            var result = await _controller.UpdateRestaurant(restaurantId, updatedRestaurant);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal("Restaurant created successfully.", apiResponse.Message);
        }

        [Fact]
        public async Task UpdateRestaurant_ReturnsNotFoundResult()
        {
            // Arrange
            var restaurantId = "invalidId";
            var updatedRestaurant = new Restaurant { /* Initialize updated restaurant properties */ };

            _restaurantServiceMock.Setup(x => x.GetRestaurantAsync(restaurantId)).ReturnsAsync((Restaurant)null);

            // Act
            var result = await _controller.UpdateRestaurant(restaurantId, updatedRestaurant);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(notFoundResult.Value);
            Assert.Equal("Invalid restaurant id.", apiResponse.Message);
        }
        #endregion

        #region DeleteRestaurant
        [Fact]
        public async Task DeleteRestaurant_ReturnsOkResult()
        {
            // Arrange
            var restaurantId = Guid.NewGuid().ToString();
            var existingRestaurant = new Restaurant { Id = restaurantId, /* Initialize restaurant properties */ };

            _restaurantServiceMock.Setup(x => x.GetRestaurantAsync(restaurantId)).ReturnsAsync(existingRestaurant);

            // Act
            var result = await _controller.DeleteRestaurant(restaurantId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal("Restaurant deleted successfully.", apiResponse.Message);
        }

        [Fact]
        public async Task DeleteRestaurant_ReturnsNotFoundResult()
        {
            // Arrange
            var restaurantId = "invalidId";

            _restaurantServiceMock.Setup(x => x.GetRestaurantAsync(restaurantId)).ReturnsAsync((Restaurant)null);

            // Act
            var result = await _controller.DeleteRestaurant(restaurantId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(notFoundResult.Value);
            Assert.Equal("Invalid restaurant id.", apiResponse.Message);
        }
        #endregion

        #region UpdateRestaurantRatings
        [Fact]
        public async Task UpdateRestaurantRatings_ReturnsOkResult()
        {
            // Arrange
            var restaurantId = Guid.NewGuid().ToString();
            var rating = 4;
            var existingRestaurant = new Restaurant { Id = restaurantId, /* Initialize restaurant properties */ };

            _restaurantServiceMock.Setup(x => x.GetRestaurantAsync(restaurantId)).ReturnsAsync(existingRestaurant);

            // Act
            var result = await _controller.UpdateRestaurantRatings(restaurantId, rating);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(okResult.Value);
            Assert.Equal("Rating updated successfully.", apiResponse.Message);
        }

        [Fact]
        public async Task UpdateRestaurantRatings_ReturnsNotFoundResult()
        {
            // Arrange
            var restaurantId = "invalidId";
            var rating = 4;

            _restaurantServiceMock.Setup(x => x.GetRestaurantAsync(restaurantId)).ReturnsAsync((Restaurant)null);

            // Act
            var result = await _controller.UpdateRestaurantRatings(restaurantId, rating);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var apiResponse = Assert.IsType<ApiResponse>(notFoundResult.Value);
            Assert.Equal("Invalid restaurant id.", apiResponse.Message);
        }
        #endregion
    }

}
