using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //test_insert();
            //test_mult_insert();
            // test_del();
            //test_update();
            //test_mult_update();
            //test_select_one();
            //test_select_list();
            test_select_content_with_comment();
            Console.WriteLine("Hello World!");
        }

        static void test_select_content_with_comment()
        {
            using (var conn=new SqlConnection("Data Source=.;Initial Catalog=dappercms;User ID=sa;Password=lsb@1972;"))
            {
                string sql_select = @"select * from content where id=@id;select * from comment where content_id=@id";
                using (var result=conn.QueryMultiple(sql_select,new { id=5}))
                {
                    var content = result.ReadFirstOrDefault<ContentWithComment>();
                    content.comments = result.Read<Comment>();
                    Console.WriteLine($"test_select_content_with_comment:内容5的评论数量：{content.comments.Count()}");
                }
            }
        }

        static void test_select_list()
        {
            using (var conn = new SqlConnection("Data Source=.;Initial Catalog=dappercms;User ID=sa;Password=lsb@1972;"))
            {
                string sql_select = @"select * from content where id in @ids";
                var result = conn.Query<Content>(sql_select, new { ids = new int[] { 4, 5 } });
                Console.WriteLine($"test_select_one:查询到的数据为：{result}");
            }
        }

        static void test_select_one()
        {
            using (var conn=new SqlConnection("Data Source=.;Initial Catalog=dappercms;User ID=sa;Password=lsb@1972;"))
            {
                string sql_select = @"select * from content where id=@id";
                var result = conn.QueryFirstOrDefault(sql_select, new { id = 5 });
                Console.WriteLine($"test_select_one:查询到的数据为：{result}");
            }
        }

        static void test_mult_update()
        {
            List<Content> contents = new List<Content>()
            {
                new Content
                {
                    id = 3,
                    title = "标题3修改了",
                    content = "内容3修改了"
                },
                new Content
                {
                    id = 4,
                    title = "标题4修改了",
                    content = "内容4修改了"
                },
            };
            
            using (var conn = new SqlConnection("Data Source=.;Initial Catalog=dappercms;User ID=sa;Password=lsb@1972;"))
            {
                string sql_update = @"update [content] set title=@title,[content]=@content,modify_time=getdate() where (id=@id)";
                var result = conn.Execute(sql_update, contents);
                Console.WriteLine($"test_mult_update:修改了{result}条数据");
            }
        }

        static void test_update()
        {
            var content = new Content
            {
                id = 5,
                title = "标题5修改了",
                content = "内容5修改了"
            };
            using (var conn=new SqlConnection("Data Source=.;Initial Catalog=dappercms;User ID=sa;Password=lsb@1972;"))
            {
                string sql_update = @"update [content] set title=@title,[content]=@content,modify_time=getdate() where (id=@id)";
                var result = conn.Execute(sql_update, content);
                Console.WriteLine($"test_update:修改了{result}条数据");
            }
        }

        static void test_del()
        {
            var content = new Content { id = 2 };
            using (var conn = new SqlConnection("Data Source=.;Initial Catalog=dappercms;User ID=sa;Password=lsb@1972;"))
            {
                string sql_del = @"delete from [content] where (id=@id)";
                var result = conn.Execute(sql_del, content);
                Console.WriteLine($"test_del:删除了{result}条数据");
            }
        }

        static void test_mult_insert()
        {
            List<Content> contents = new List<Content>()
            {
                new Content
                {
                    title="批量插入标题3",
                    content="批量插入内容3",
                },
                 new Content
                {
                    title="批量插入标题4",
                    content="批量插入内容4",
                }
            };

            using (var conn = new SqlConnection("Data Source=.;Initial Catalog=dappercms;User ID=sa;Password=lsb@1972;"))
            {
                string sql_insert = @"insert into [content](title,[content],status,add_time,modify_time) values(@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, contents);
                Console.WriteLine($"test_mult_insert:插入了{result}条数据！");
            }

        }


        /// <summary>
        /// 测试插入单条数据
        /// </summary>
        static void test_insert()
        {
            var content = new Content
            {
                title = "标题1",
                content = "内容1"
            };
            using (var conn=new SqlConnection("Data Source=.;Initial Catalog=dappercms;User ID=sa;Password=lsb@1972;"))
            {
                string sql_insert = @"insert into [content](title,[content],status,add_time,modify_time) values(@title,@content,@status,@add_time,@modify_time)";
                var result = conn.Execute(sql_insert, content);
                Console.WriteLine($"test_insert:插入了{result}条数据！");
            }
        }
    }

    public class Content
    {
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public int status { get; set; }
        public DateTime add_time { get; set; } = DateTime.Now;
        public DateTime? modify_time { get; set; }
    }

    public class Comment
    {
        public int id { get; set; }
        public int content_id { get; set; }
        public string content { get; set; }
        public DateTime add_time { get; set; } = DateTime.Now;
    }

    public class ContentWithComment
    {
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime add_time { get; set; } = DateTime.Now;
        public DateTime? modify_time { get; set; }
        public IEnumerable<Comment> comments { get; set; }
    }
}
