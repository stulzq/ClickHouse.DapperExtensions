using System;
using System.Collections.Generic;
using ClickHouse.Ado;
using Dapper;

namespace ClickHouse.DapperExtensions.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var conn = new ClickHouseConnection("Compress=True;CheckCompressedHash=False;Compressor=lz4;Host=192.168.10.110;Port=9000;Database=geekbuying;User=default;Password=;");
            conn.Open();
            CreateTable(conn);

            try
            {
                var user=new TestUser(){ResisterDate = DateTime.Now,ResisterTime = DateTime.Now,Age = 18,Name = "张三2"};
                var user2=new TestUser(){ResisterDate = DateTime.Now,ResisterTime = DateTime.Now,Age = 18,Name = "张三"};
//                conn.Insert(user);
                Console.WriteLine("Insert single success.");

                conn.InsertBulk(new List<TestUser>(){ user, user2 });
                Console.WriteLine("Insert multi success.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            conn.Close();
        }

        static void CreateTable(ClickHouseConnection conn)
        {
            conn.Execute(
                "CREATE TABLE IF NOT EXISTS TestUser (ResisterDate Date, ResisterTime DateTime, Name String, Age UInt16) ENGINE=MergeTree(ResisterDate,(ResisterTime,Name,Age), 8192)");
            Console.WriteLine("Create table 'TestUser' success.");
        }
    }
}
