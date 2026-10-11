//using HardwareStore.Models;
//using HardwareStore.SeedWork;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Storage;

//namespace RedditQuestion
//{

//    IExecutionStrategy strategy = _context.Database.CreateExecutionStrategy();
//    await strategy.ExecuteAsync(async () =>
//            {
//        await using var transaction = await _context.Database.BeginTransactionAsync();

//         // Delete all the content from the database
//         // _context.AddRange(X);
//         // _contextSaveChangesAsync()
//         // _context.AddRange(Y)
//         //_context.SaveChangesAsync()

//        await transaction.CommitAsync();
//    });

//}
