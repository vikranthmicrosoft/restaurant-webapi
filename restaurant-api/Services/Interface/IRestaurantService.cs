using restaurant_api.Model;

namespace restaurant_api.Services.Interface
{
    public interface IRestaurantService
    {
        Task AddUpdateRestaurantAsync(Restaurant restaurant);
        Task<Restaurant> GetRestaurantAsync(string id);
        Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
        Task DeleteRestaurant(Restaurant restaurant);
        Task UpdateRestaurantRatings(Restaurant restaurant, int rating);
    }
}
