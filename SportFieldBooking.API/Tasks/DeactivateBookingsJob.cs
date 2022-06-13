using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportFieldBooking.Biz;
using SportFieldBooking.Biz.Booking;
        
namespace SportFieldBooking.API.Tasks
{
    public class DeactivateBookingsJob : IJob
    {
        private readonly IRepositoryWrapper _repository;
        public DeactivateBookingsJob(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        public Task Execute(IJobExecutionContext context)
        {
            var task = Task.Run(() => _repository.Booking.DeactivateExpiredBookings());
            //var task = Task.Run(() => Console.WriteLine("Deactivate expired bookings..."));
            return task;
        }
    }
}
