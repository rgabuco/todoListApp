using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace PROJ.Services
{
    public class WeatherService
    {
        private Panel weatherPanel;
        private bool isOffline;

        public WeatherService(Panel weatherPanel, bool isOffline)
        {
            this.weatherPanel = weatherPanel;
            this.isOffline = isOffline;
        }

        public async Task LoadWeatherData()
        {
            if (isOffline)
            {
                // Use dummy data when offline
                string dummyJson = @"
                {
                    'forecast': {
                        'forecastday': [
                            {
                                'date': '2023-10-01',
                                'day': {
                                    'avgtemp_c': 15.0,
                                    'condition': {
                                        'text': 'Sunny',
                                        'icon': '//cdn.weatherapi.com/weather/64x64/day/113.png'
                                    }
                                }
                            },
                            {
                                'date': '2023-10-02',
                                'day': {
                                    'avgtemp_c': 17.0,
                                    'condition': {
                                        'text': 'Partly cloudy',
                                        'icon': '//cdn.weatherapi.com/weather/64x64/day/116.png'
                                    }
                                }
                            },
                            {
                                'date': '2023-10-03',
                                'day': {
                                    'avgtemp_c': 14.0,
                                    'condition': {
                                        'text': 'Rainy',
                                        'icon': '//cdn.weatherapi.com/weather/64x64/day/308.png'
                                    }
                                }
                            }
                        ]
                    }
                }";
                JObject data = JObject.Parse(dummyJson);
                DisplayWeatherData(data);
            }
            else
            {
                string apiKey = Environment.GetEnvironmentVariable("API_KEY") ??
                               throw new InvalidOperationException("Weather API key is not set.");

                if (string.IsNullOrEmpty(apiKey))
                {
                    MessageBox.Show("Weather API key is not set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                string city = "Calgary";
                string url = $"http://api.weatherapi.com/v1/forecast.json?key={apiKey}&q={city}&days=3";

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        HttpResponseMessage response = await client.GetAsync(url);
                        if (response.IsSuccessStatusCode)
                        {
                            string json = await response.Content.ReadAsStringAsync();
                            JObject data = JObject.Parse(json);
                            DisplayWeatherData(data);
                        }
                        else
                        {
                            DisplayErrorMessage("Failed to fetch weather data.");
                        }
                    }
                    catch (Exception ex)
                    {
                        DisplayErrorMessage($"Exception: {ex.Message}");
                    }
                }
            }
        }

        private void DisplayWeatherData(JObject data)
        {
            // Clear existing controls in the weather panel
            weatherPanel.Controls.Clear();

            // Set the weather panel size and position
            int weatherPanelWidth = 343;
            int weatherPanelHeight = 165; // Adjusted to accommodate content
            weatherPanel.Size = new Size(weatherPanelWidth, weatherPanelHeight);
            weatherPanel.BackColor = Color.Transparent;

            // Specify the position of the first dayPanel
            int firstPanelX = 10; // X-coordinate of the top-left corner
            int firstPanelY = 20; // Y-coordinate of the top-left corner

            // Define dayPanel dimensions and spacing
            int dayPanelWidth = 100; // Width of each dayPanel
            int dayPanelHeight = 130; // Increased height to fit all content
            int spacing = 10; // Space between day panels

            JArray days = (JArray)data["forecast"]["forecastday"];

            for (int i = 0; i < 3; i++) // Loop through the first 3 days
            {
                JObject day = (JObject)days[i];
                string date = DateTime.Parse((string)day["date"]).ToString("ddd");
                string icon = (string)day["day"]["condition"]["icon"];
                string description = (string)day["day"]["condition"]["text"];
                string temp = ((double)day["day"]["avgtemp_c"]).ToString("0.0") + "°C";

                Panel dayPanel = new Panel
                {
                    Width = dayPanelWidth,
                    Height = dayPanelHeight,
                    BackColor = Color.LightSkyBlue,
                    BorderStyle = BorderStyle.FixedSingle,

                    // Set the position of the current dayPanel
                    Location = new Point(
                        firstPanelX + (dayPanelWidth + spacing) * i, // X-coordinate with spacing
                        firstPanelY // Y-coordinate remains the same
                    )
                };

                PictureBox iconBox = new PictureBox
                {
                    ImageLocation = $"http:{icon}",
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Size = new Size(40, 40), // Adjust icon size
                    Location = new Point((dayPanel.Width - 40) / 2, 5)
                };

                Label dateLabel = new Label
                {
                    Text = date,
                    Font = new Font("Arial", 9, FontStyle.Bold),
                    Location = new Point(0, 50),
                    Width = dayPanel.Width,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                Label descriptionLabel = new Label
                {
                    Text = description,
                    Font = new Font("Arial", 8),
                    Location = new Point(0, 70),
                    Width = dayPanel.Width,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                Label tempLabel = new Label
                {
                    Text = temp,
                    Font = new Font("Arial", 9, FontStyle.Bold),
                    ForeColor = Color.DarkBlue,
                    Location = new Point(0, 95),
                    Width = dayPanel.Width,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                // Add controls to the day panel
                dayPanel.Controls.Add(iconBox);
                dayPanel.Controls.Add(dateLabel);
                dayPanel.Controls.Add(descriptionLabel);
                dayPanel.Controls.Add(tempLabel);

                // Add day panel to the weather panel
                weatherPanel.Controls.Add(dayPanel);
            }

            // Add the weather panel to the form (if not already added)
            if (!weatherPanel.Parent.Controls.Contains(weatherPanel))
            {
                weatherPanel.Parent.Controls.Add(weatherPanel);
            }
        }

        private void DisplayErrorMessage(string message)
        {
            Label errorLabel = new Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Red
            };
            weatherPanel.Controls.Add(errorLabel);
        }
    }
}
