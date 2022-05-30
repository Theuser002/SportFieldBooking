use SportFieldBooking

delete from Users

select * from Users
select * from SportFields
select * from Bookings
select * from BookingStatuses

alter table Users drop constraint [DF__Users__Role__17036CC0];
alter table Users drop column Role

drop index IX_BookingStatuses_StatusName on BookingStatuses
drop index IX_Users_Username on Users