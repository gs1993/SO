using Amazon.SQS;

namespace PostPublisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IAmazonSQS sqsClient = new AmazonSQSClient();
        }
    }
}