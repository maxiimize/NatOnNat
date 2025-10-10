using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherDto?> GetCurrentWeatherAsync(string city = "Stockholm");
    }
}
