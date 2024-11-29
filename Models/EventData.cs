using MySql.Data.MySqlClient;


namespace CalendarApp.Models
{

    public class EventData
    {
        private readonly string _connectionString;

        public EventData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Retrieve events from the calendar database
        public List<Event> GetAllEvents()
        {
            var events = new List<Event>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT * FROM Events";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events.Add(new Event
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.GetString(2),
                                StartTime = reader.GetDateTime(3),
                                EndTime = reader.GetDateTime(4)
                            });
                        }
                    }
                }
            }
            return events;
        }

        // Add a new event to the calendar database
        public void AddEvent(Event calendarEvent)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "INSERT INTO Events (Title, StartTime, EndTime, Description) VALUES (@Title, @StartTime, @EndTime, @Description)";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Title", calendarEvent.Title);
                    cmd.Parameters.AddWithValue("@StartTime", calendarEvent.StartTime);
                    cmd.Parameters.AddWithValue("@EndTime", calendarEvent.EndTime);
                    cmd.Parameters.AddWithValue("@Description", calendarEvent.Description);
                    cmd.ExecuteNonQuery();
                }
            }
        }




        public Event GetEventById(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT Event_id, Title, Description, StartTime, EndTime FROM Events WHERE Event_id = @Id";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Event
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.GetString(2),
                                StartTime = reader.GetDateTime(3),
                                EndTime = reader.GetDateTime(4)


                            };
                        }
                    }
                }
                connection.Close();
            }
            return null;
        }

        public void UpdateEvent(Event calendarEvent)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var query = "UPDATE Events SET Title = @UpdatedTitle, @StartTime, @EndTime, @Description WHERE Title = @Title";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Title", calendarEvent.Title);
                    cmd.Parameters.AddWithValue("@StartTime", calendarEvent.StartTime);
                    cmd.Parameters.AddWithValue("@EndTime", calendarEvent.EndTime);
                    cmd.Parameters.AddWithValue("@Description", calendarEvent.Description);
                    cmd.ExecuteNonQuery();
                }
            }
        }



    }
}
