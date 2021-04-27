using System;
using System.Linq;
using System.Net;
using System.Text;
using Amazon;
using Amazon.EC2;

namespace aws_sdk_for_net
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var config = new AmazonEC2Config
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName("us-east-2")
                };

                var client = new AmazonEC2Client(config);
                var response = client.DescribeInstances();

                if (response != null)
                {
                    if (response.HttpStatusCode == HttpStatusCode.OK)
                    {
                        if (response.DescribeInstancesResult.Reservations != null && response.DescribeInstancesResult.Reservations.Any())
                        {
                            var sBuilder = new StringBuilder();

                            foreach (var item in response.DescribeInstancesResult.Reservations)
                            {
                                sBuilder.AppendLine($"OwnerId: {item.OwnerId}");
                                sBuilder.AppendLine($"RequesterId: {item.RequesterId}");
                                sBuilder.AppendLine($"ReservationId: {item.ReservationId}");
                                sBuilder.AppendLine("GroupNames:");

                                foreach (var name in item.GroupNames)
                                {
                                    sBuilder.AppendLine($"* {name}");
                                }

                                sBuilder.AppendLine("Instances:");

                                foreach (var instance in item.Instances)
                                {
                                    sBuilder.AppendLine($"* AmiLaunchIndex: {instance.AmiLaunchIndex}; ImageId: {instance.ImageId}; KeyName: {instance.KeyName}");
                                }

                                sBuilder.AppendLine("--------------------------------------------------------");
                            }

                            Console.WriteLine(sBuilder.ToString());
                        }
                        else
                        {
                            Console.WriteLine("Reservations is null or empty.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong with the request.");
                    }
                }
                else
                {
                    Console.WriteLine("The response is null.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex}: {ex.Message}");
            }

            Console.WriteLine("\n\nTap to continue...");
            Console.ReadKey();
        }
    }
}