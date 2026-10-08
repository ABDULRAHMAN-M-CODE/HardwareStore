
namespace RedditQuestion
{
public class TResult
{
        // Instance fields....
        // No properties.
        // No methods.
}
    public interface ISomeInterface<T>
    {
        public Task<T> SomeMethod();
    }
    public class SomeClass : ISomeInterface<TResult>
    {
        // instance fields.....

        //constructor......

        public async Task<TResult> SomeMethod()
        {


            //Some code that is not important to this question.......



            TResult result = new TResult
            {
                // assign instance fields....
            };

            Task<TResult> task = Task.FromResult<TResult>(result);

            return task ;
            
            // Solution is:  return result;



        }

    }

}
