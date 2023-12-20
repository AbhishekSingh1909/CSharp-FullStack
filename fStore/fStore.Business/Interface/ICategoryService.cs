using fStore.Business.DTO;
using fStore.Core;

namespace fStore.Business;

public interface ICategoryService : IBaseService<Category, CategoryReadDTO, CategoryCreateDTO, CategoryUpdateDTO>
{

}
