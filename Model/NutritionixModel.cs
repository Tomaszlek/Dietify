using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DietMaker.Model
{
    public class NutritionixResponseModel
    {
        [JsonPropertyName("foods")]
        public List<FoodItem> foods { get; set; }
    }

    public class FoodItem
    {
        [JsonPropertyName("food_name")]
        public string food_name { get; set; }

        [JsonPropertyName("nf_calories")]
        public float nf_calories { get; set; }

        [JsonPropertyName("nf_total_fat")]
        public float nf_total_fat { get; set; }

        [JsonPropertyName("nf_total_carbohydrate")]
        public float nf_total_carbohydrate { get; set; }

        [JsonPropertyName("nf_protein")]
        public float nf_protein { get; set; }
    }
}