using AutoMapper;
using fStore.Core;

namespace fStore.Business;

public class AddressService : BaseService<Address, AddressReadDTO, AddressCreateDTO, AddressUpdateDTO>, IAddressService
{
    IAddressRepo _addressRepo;
    public AddressService(IAddressRepo repo, IMapper mapper) : base(repo, mapper)
    {
        _addressRepo = repo;
    }

    public async Task<AddressReadDTO> GetAddreess(Guid id)
    {
        var address = await _addressRepo.GetAddreess(id);

        return _mapper.Map<Address, AddressReadDTO>(address);
    }

}
