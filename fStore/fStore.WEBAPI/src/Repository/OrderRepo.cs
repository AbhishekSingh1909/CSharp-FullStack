using System.Data.Common;
using fStore.Business;
using fStore.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;

namespace fStore.WEBAPI;

public class OrderRepo : BaseRepo<Order>, IOrderRepo
{
    private readonly DbSet<Product> _product;
    public OrderRepo(DataBaseContext dataBaseContext) : base(dataBaseContext)
    {
        _product = dataBaseContext.Products;
    }

    public override async Task<IEnumerable<Order>> GetAllAsync(GetAllParams options)
    {
        var orders = await _data.AsNoTracking().Include(o => o.OrderDetails).Skip(options.Offset).Take(options.Limit).ToArrayAsync();
        return orders;
    }

    public override async Task<Order?> GetByIdAsync(Guid id)
    {
        var order = await _data.AsNoTracking().Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == id);
        return order;
    }

    public override async Task<Order> CreateOneAsync(Order createObject)
    {
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                foreach (var op in createObject.OrderDetails)
                {
                    var product = await _product.AsNoTracking().FirstOrDefaultAsync(p => p.Id == op.ProductId);
                    if (product.Inventory >= op.Quntity)
                    {
                        Console.WriteLine($"BEFORE ____ {product.Inventory}");
                        product.Inventory -= op.Quntity;
                        Console.WriteLine($"AFTER ____ {product.Inventory}");
                        op.Price = product.Price;
                        op.CreatedAt = DateTime.Now;
                        op.UpdatedAt = DateTime.Now;
                        _product.Update(product);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        throw CustomException.InsufficientInventory($"Could not create order: not enough products in inventory. Product: {product!.Title}, Inventory: {product.Inventory}, Ordered amount: {op.Quntity}");
                    }
                }
                createObject.OrderStatus = OrderStatus.Pending;
                await _data.AddAsync(createObject);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return createObject;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await transaction.RollbackAsync();
                throw new Exception("can't create order");
            }
        }
    }

    public override async Task<Order> UpdateOneAsync(Guid id, Order updateObject)
    {
        _data.Update(updateObject);
        await _dbContext.SaveChangesAsync();
        return updateObject;
    }

    public async Task<IEnumerable<Order>> GetUserAllOrdersAsync(Guid id)
    {
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var orders = await _data.AsNoTracking().Include(o => o.OrderDetails).Where(o => o.UserId == id).ToArrayAsync();
                await transaction.CommitAsync();
                return orders;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                await transaction.RollbackAsync();
                throw new Exception("can't get orders for User");
            }
        }

    }
}
