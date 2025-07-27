using BookingDomain.Entities;

namespace BookingApp.Common.Contracts;

public interface IInMemoryHomeStore
{
  public IEnumerable<Home> GetAllHomes();
}