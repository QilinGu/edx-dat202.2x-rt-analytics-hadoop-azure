﻿using System;
using System.Text;
using Microsoft.HBase.Client;
using org.apache.hadoop.hbase.rest.protobuf.generated;

namespace StockReader
{
    class Program
    {
        static void Main(string[] args)
        {

            bool quit = false;
            while (!quit)
            {
                Console.ResetColor();
                Console.WriteLine("Enter a stock code, or enter 'quit' to exit");

                // Connect to HBase cluster
                string clusterURL = "https://hb12345.azurehdinsight.net";
                string userName = "HDUser";
                string password = "HDPa$$w0rd";

                ClusterCredentials creds = new ClusterCredentials(new Uri(clusterURL), userName, password);
                HBaseClient hbaseClient = new HBaseClient(creds);

                string input = Console.ReadLine();
                if (input.ToLower() == "quit")
                {
                    quit = true;
                }
                else
                {
                    CellSet cellSet = hbaseClient.GetCells("Stocks", input);
                    var row = cellSet.rows[0];
                    Double currentPrice = Double.Parse(Encoding.UTF8.GetString(row.values[1].data));
                    Double closingPrice = Double.Parse(Encoding.UTF8.GetString(row.values[0].data));
                    if (currentPrice > closingPrice)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                        if (currentPrice < closingPrice)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.WriteLine(input + ": " + currentPrice.ToString());
                }
            }

        }
    }
}
