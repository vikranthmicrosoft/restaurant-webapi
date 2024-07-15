using Amazon.DynamoDBv2.DataModel;

namespace restaurant_api.Model
{
    [DynamoDBTable("Restaurants")]
    public class Restaurant
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string Name { get; set; }

        [DynamoDBProperty]
        public string EmailAddress { get; set; }

        [DynamoDBProperty]
        public string Address { get; set; }

        [DynamoDBProperty]
        public string Description { get; set; }
        [DynamoDBProperty]
        public string Hours { get; set; }
        [DynamoDBProperty]
        public List<NumberList> Ratings { get; set; }
        [DynamoDBProperty]
        public double AverageRating { get; set; }
    }

    public class NumberList
    {
        public int Number { get; set; }

    }
}
