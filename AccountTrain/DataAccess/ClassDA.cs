using Common;
using DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using BusinessEntitys;
using BusinessEntity.Model;

namespace DataAccess
{
    public class ClassDA
    {

        #region 课程管理

        public List<ClassEntity> GetAllClass()
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_Class
                                            where status=1 order by createtime desc");
                return conn.Query<ClassEntity>(query).ToList();
            }
        }

        public List<ClassEntity> GetClassByType(string type)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_Class
                                            where status=1 and ClassType = '{0}' order by createtime desc", type);
                return conn.Query<ClassEntity>(query).ToList();
            }
        }

        public List<ClassEntity> GetNewestClass()
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select top 6 * from Train_Class
                                            where status=1 and ClassType in ('1','2')  order by createtime desc");
                return conn.Query<ClassEntity>(query).ToList();
            }
        }

        public List<ClassEntity> GetClassByCondition(string name, string classType, string startDate, string endDate,string classGroup,string order)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_Class
                                where 1=1");
                if (!string.IsNullOrEmpty(name))
                {
                    string sql = string.Format(" and ClassName like '%{0}%'", name);
                    query = query + sql;
                }
                if (!string.IsNullOrEmpty(classType))
                {
                    string sql = string.Format(" and ClassType='{0}'", classType);
                    query = query + sql;
                }
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    string sql = string.Format(" and CreateTime between '{0}' and '{1}'", startDate, endDate);
                    query = query + sql;
                }
                if (!string.IsNullOrEmpty(classGroup))
                {
                    string sql = string.Format(" and ClassGroup='{0}'", classGroup);
                    query = query + sql;
                }
                if (!string.IsNullOrEmpty(order))
                {
                    string sql = string.Format(" order by {0} desc", order);
                    query = query + sql;
                }

                return conn.Query<ClassEntity>(query).ToList();

            }
        }

        public ClassEntity GetClassByKey(string id)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_Class
                                            where ClassId='{0}'", id);
                return conn.Query<ClassEntity>(query).FirstOrDefault();
            }
        }


        public int SaveClass(ClassEntity Class, string loginName)
        {
            if (string.IsNullOrEmpty(Class.ClassId))
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"INSERT INTO Train_Class
                                                   (ClassId
                                                   ,ClassName
                                                   ,ClassType
                                                   ,ClassContent
                                                   ,ClassImages
                                                   ,ClassPrice
                                                   ,ClassTeacher
                                                   ,HotCount
                                                   ,IsGroupBuy
                                                   ,IsBargain
                                                   ,IsHelp
                                                   ,Remark                                                   
                                                   ,Status
                                                   ,CreateTime
                                                   ,CreateUser
                                                   ,UpdateTime
                                                   ,UpdateUser
                                                   ,ClassAbstract,ClassGroup)
                                             VALUES
                                                    ('{0}'
                                                    ,'{1}'
                                                    ,'{2}'
                                                    ,'{3}'
                                                    ,'{4}'
                                                    ,'{5}'
                                                    ,'{6}'
                                                    ,'{7}'
                                                    ,'{8}'
                                                    ,'{9}'
                                                    ,'{10}'
                                                    ,'{11}'
                                                    ,'{12}'
                                                    ,getdate()
                                                    ,'{13}'
                                                    ,getdate()
                                                    ,'{14}'
                                                    ,'{15}','{16}')",
                        Guid.NewGuid().ToString(), Class.ClassName, Class.ClassType, Class.ClassContent, Class.ClassImages, Class.ClassPrice,
                        Class.ClassTeacher, Class.HotCount, Class.IsGroupBuy, Class.IsBargain, Class.IsHelp, Class.Remark, 1, loginName, loginName, Class.ClassAbstract,Class.ClassGroup);
                    return conn.Execute(query);
                }
            }
            else
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@" UPDATE Train_Class
                                                    SET ClassName= '{0}'
                                                       ,ClassType= '{1}'
                                                       ,ClassContent= '{2}'
                                                       ,ClassImages= '{3}'
                                                       ,ClassPrice= '{4}'
                                                       ,ClassTeacher= '{5}'
                                                       ,HotCount= '{6}'
                                                       ,IsGroupBuy= '{7}'
                                                       ,IsBargain= '{8}'
                                                       ,IsHelp= '{9}'
                                                       ,Remark = '{10}'                                                             
                                                       ,UpdateTime = getdate()
                                                       ,UpdateUser = '{11}'
                                                       ,ClassAbstract = '{12}',ClassGroup='{13}'
                                                    WHERE ClassId='{14}'",
                                              Class.ClassName, Class.ClassType, Class.ClassContent, Class.ClassImages, Class.ClassPrice, Class.ClassTeacher, Class.HotCount, Class.IsGroupBuy,
                                              Class.IsBargain, Class.IsHelp, Class.Remark, loginName, Class.ClassAbstract,Class.ClassGroup, Class.ClassId);
                    return conn.Execute(query);
                }
            }
        }

        public int EnableClass(string ClassId, int status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_Class set status={0}  where ClassId='{1}'", status, ClassId);
                return conn.Execute(query);
            }
        }


        public List<VMOrderClass> GetMyClassByopenId(string openid, string title)
        {

            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@" select t2.*,t.status as OrderStatus,t.OrderNo
                                                from Train_Order t
                                                inner join Train_OrderGoods t1 on t.OrderId=t1.OrderId
                                                inner join Train_Class t2 on t1.ClassId=t2.ClassId
                                                where t.Openid='{0}'", openid);
                if (!string.IsNullOrEmpty(title))
                {
                    string sql = string.Format(" and t2.ClassName='{0}'", title);
                    query = query + sql;
                }
                return conn.Query<VMOrderClass>(query).ToList();
            }
           
        }
        /// <summary>
        /// 更新课程热度（即购买人数）
        /// </summary>
        /// <param name="classId"></param>
        /// <returns></returns>
        public int UpdateClassHot(string classId)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_Class set HotCount=HotCount+1 where ClassId='{0}'", classId);
                return conn.Execute(query);
            }
        }

      
        #endregion

        #region 章节管理
        public List<VMClassChapter> GetChaptersByCondition(string className, string chapterTitle)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.*,t1.ClassName 
                                               from Train_Chapter t
                                               inner join Train_Class t1 on t.ClassId=t1.ClassId
                                               where t1.status=1 and t.status=1");
                if (!string.IsNullOrEmpty(className))
                {
                    string sql = string.Format(" and t1.ClassName like '%{0}%'", className);
                    query = query + sql;
                }

                if (!string.IsNullOrEmpty(chapterTitle))
                {
                    string sql = string.Format(" and t.ChapterTitle like '%{0}%'", chapterTitle);
                    query = query + sql;
                }

                return conn.Query<VMClassChapter>(query).ToList();

            }
        }


       
        /// <summary>
        /// 根据键值key获取items
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<ChapterEntity> GetChaptersByClassKey(string key)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_Chapter
                                            where ClassId='{0}' and status=1
                                            order by ChapterNum", key);
                return conn.Query<ChapterEntity>(query).ToList();
            }
        }

        public ChapterEntity GetChapterByKey(string id)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_Chapter
                                            where ChapterId='{0}'", id);
                return conn.Query<ChapterEntity>(query).FirstOrDefault();
            }
        }


        public int SaveChapter(ChapterEntity entity, string loginName)
        {
            if (string.IsNullOrEmpty(entity.ChapterId))
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"INSERT INTO Train_Chapter
                                                    (ChapterId
                                                    ,ClassId
                                                    ,ChapterTitle
                                                    ,ChapterContent
                                                    ,VideoUrl
                                                    ,VideoTitle
                                                    ,ChapterNum
                                                    ,Remark
                                                    ,Status
                                                    ,CreateTime
                                                    ,CreateUser
                                                    ,UpdateTime
                                                    ,UpdateUser)
                                                VALUES
                                                    ('{0}'
                                                    ,'{1}'
                                                    ,'{2}'
                                                    ,'{3}'
                                                    ,'{4}'
                                                    ,'{5}'
                                                    ,'{6}'
                                                    ,'{7}'
                                                    ,'{8}'
                                                    ,getdate()
                                                    ,'{9}'
                                                    ,getdate()
                                                    ,'{10}')",
                        Guid.NewGuid().ToString(), entity.ClassId, entity.ChapterTitle, entity.ChapterContent, entity.VideoUrl, entity.VideoTitle,
                        entity.ChapterNum, entity.Remark, 1, loginName, loginName);
                    return conn.Execute(query);
                }
            }
            else
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@" UPDATE Train_Chapter
                                                    SET  ClassId='{0}'
                                                        ,ChapterTitle='{1}'
                                                        ,ChapterContent='{2}'
                                                        ,VideoUrl='{3}'
                                                        ,VideoTitle='{4}'
                                                        ,ChapterNum='{5}'
                                                        ,Remark='{6}'
                                                        ,UpdateTime = getdate()
                                                        ,UpdateUser = '{7}'
                                                    WHERE ChapterId='{8}'",
                                               entity.ClassId, entity.ChapterTitle, entity.ChapterContent, entity.VideoUrl, entity.VideoTitle, entity.ChapterNum,
                                               entity.Remark, loginName,entity.ChapterId);
                    return conn.Execute(query);
                }
            }
        }

        public int EnableChapter(string id, int status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_Chapter set status={0}  where ChapterId='{1}'", status, id);
                return conn.Execute(query);
            }
        }
        #endregion


       


    }
}
