using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using restaurant_api.Common.Service.Interface;
using restaurant_api.Model;
using restaurant_api.Services.Interface;

namespace restaurant_api.Services.Implementation
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly Amazon.DynamoDBv2.DataModel.DynamoDBContext _context;
        private readonly IEmailService _emailService;

        public RestaurantService(IAmazonDynamoDB dynamoDbClient, IEmailService emailService)
        {
            _dynamoDbClient = dynamoDbClient;
            _context = new DynamoDBContext(_dynamoDbClient);
            _emailService = emailService;
        }
        public async Task AddUpdateRestaurantAsync(Restaurant restaurant)
        {
            restaurant.AverageRating = 0;
            if (restaurant.Ratings != null && restaurant.Ratings.Any())
            {
                restaurant.AverageRating = restaurant.Ratings.Select(x => x.Number).Average();
            }
            else
            {
                restaurant.Ratings = new List<NumberList>();
            }
            await _context.SaveAsync(restaurant);
        }

        public async Task<Restaurant> GetRestaurantAsync(string id)
        {
            return await _context.LoadAsync<Restaurant>(id);
        }

        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            var conditions = new List<ScanCondition>();
            return await _context.ScanAsync<Restaurant>(conditions).GetRemainingAsync();
        }

        public async Task DeleteRestaurant(Restaurant restaurant)
        {
            await _context.DeleteAsync<Restaurant>(restaurant);
        }

        public async Task UpdateRestaurantRatings(Restaurant restaurant, int rating)
        {
            if (restaurant.Ratings == null)
            {
                restaurant.Ratings = new List<NumberList>();
            }
            restaurant.Ratings.Add(new NumberList()
            {
                Number = rating
            });

            restaurant.AverageRating = 0;
            if (restaurant.Ratings.Any())
            {
                restaurant.AverageRating = restaurant.Ratings.Select(x => x.Number).Average();
            }

            if (restaurant.Ratings.Count >= 5)
            {
                var lastfiveRatings = restaurant.Ratings.Reverse<NumberList>().Take(5).Select(x => x.Number).Sum() / 5;

                if (lastfiveRatings > 0 && lastfiveRatings < restaurant.AverageRating)
                {

                    var htmlBody = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Restaurant Feedback Alert</title>\r\n</head>\r\n<body>\r\n    <div style=\"font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;\">\r\n        <h2 style=\"color: #333;\">Restaurant Feedback Alert</h2>\r\n        \r\n        <p>Dear Restaurant Management,</p>\r\n        \r\n        <p>We are writing to inform you about recent customer feedback. Unfortunately, the last 5 ratings received have been less than average ratings.</p>\r\n        \r\n        <p>We highly value your restaurant's reputation and customer satisfaction. Please take this feedback into consideration and address any issues that may have contributed to these ratings.</p>\r\n        \r\n        <p>Thank you for your attention to this matter.</p>\r\n        \r\n        <p style=\"font-size: 0.8em; color: #999;\">This message was sent to you based on automated feedback analysis.</p>\r\n    </div>\r\n</body>\r\n</html>\r\n";

                    await _emailService.SendEmailAsync(restaurant.EmailAddress, "Restaurant Feedback Alert", htmlBody, "");
                }

            }

            await _context.SaveAsync(restaurant);
        }
    }
}
