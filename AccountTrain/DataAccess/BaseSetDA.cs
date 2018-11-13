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
    public class BaseSetDA
    {
        #region 公司信息
        public CompanyEntity GetCompanyInfo()
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format("SELECT * FROM Train_CompanyInfo ");
                return conn.Query<CompanyEntity>(query).FirstOrDefault();
            }
        }


        public int SaveCompanyInfo(CompanyEntity company, string loginName)
        {
            if (string.IsNullOrEmpty(company.CompanyId))
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"INSERT INTO Train_CompanyInfo
                                                   (CompanyId
                                                   ,CompanyName
                                                   ,CompanyAddress
                                                   ,CompanyTelephone
                                                   ,CompanyEmail
                                                   ,ContactName
                                                   ,ContactPhone
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
                        Guid.NewGuid().ToString(), company.CompanyName, company.CompanyAddress, company.CompanyTelephone, company.CompanyEmail, company.CompanyName,
                        company.ContactPhone, company.Remark, 1, loginName, loginName);
                    return conn.Execute(query);
                }
            }
            else
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"UPDATE Train_CompanyInfo
                                                   SET CompanyName = '{0}'
                                                      ,CompanyAddress = '{1}'
                                                      ,CompanyTelephone = '{2}'
                                                      ,CompanyEmail ='{3}'
                                                      ,ContactName = '{4}'
                                                      ,ContactPhone ='{5}'
                                                      ,Remark = '{6}'
                                                      ,UpdateTime =getdate()
                                                      ,UpdateUser = '{7}'
                                                 WHERE CompanyId='{8}'",
                                                company.CompanyName, company.CompanyAddress, company.CompanyTelephone, company.CompanyEmail, company.CompanyName,
                                                company.ContactPhone, company.Remark, loginName, company.CompanyId);
                    return conn.Execute(query);
                }
            }
        }
        #endregion

        #region 招聘信息
        public List<JobEntity> GetJobListByCondition(string title)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_Job
                                            where status=1");
                if (!string.IsNullOrEmpty(title))
                {
                    string sql = string.Format(" and JobTitle='{0}'", title);
                    query = query + sql;
                }

                return conn.Query<JobEntity>(query).ToList();

            }
        }

        public JobEntity GetJobInfoByKey(string id)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_Job
                                            where JobId='{0}'", id);
                return conn.Query<JobEntity>(query).FirstOrDefault();
            }
        }


        public int SaveJobInfo(JobEntity job, string loginName)
        {
            if (string.IsNullOrEmpty(job.JobId))
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"INSERT INTO Train_Job
                                                   (JobId
                                                   ,JobTitle
                                                   ,JobDesc
                                                   ,JobRequirements
                                                   ,JobSalary
                                                   ,JobYear
                                                   ,JobEducation
                                                   ,Remark
                                                   ,Status
                                                   ,CreateTime
                                                   ,CreateUser
                                                   ,UpdateTime
                                                   ,UpdateUser
                                                   ,JobCompany
                                                   ,JobEmail)
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
                                                   ,'{10}'
                                                   ,'{11}'
                                                   ,'{12}')",
                        Guid.NewGuid().ToString(),job.JobTitle,job.JobDesc,job.JobRequirements,job.JobSalary,
                        job.JobYear, job.JobEducation, job.Remark, 1, loginName, loginName, job.JobCompany, job.JobEmail);
                    return conn.Execute(query);
                }
            }
            else
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"UPDATE Train_Job
                                                   SET JobTitle = '{0}'
                                                      ,JobDesc = '{1}'
                                                      ,JobRequirements = '{2}'
                                                      ,JobSalary = '{3}'
                                                      ,JobYear ='{4}'
                                                      ,JobEducation ='{5}'
                                                      ,Remark = '{6}'
                                                      ,JobCompany = '{7}'
                                                      ,JobEmail = '{8}'
                                                      ,UpdateTime = getdate()
                                                      ,UpdateUser = '{9}'
                                                 WHERE JobId='{10}'",
                                               job.JobTitle, job.JobDesc, job.JobRequirements, job.JobSalary,job.JobYear,
                                               job.JobEducation, job.Remark, job.JobCompany, job.JobEmail, loginName, job.JobId);
                    return conn.Execute(query);
                }
            }
        }

        public int EnableJob(string jobId, int status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_Job set status={0}  where JobId='{1}'", status, jobId);
                return conn.Execute(query);
            }
        }
        #endregion

        #region 字典信息
        public List<DictionaryItemEntity> GetDicListByCondition(string dk, string ik, string iv)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_DictionaryItems
                                            where status=1");
                if (!string.IsNullOrEmpty(dk))
                {
                    string sql = string.Format(" and DictionaryKey='{0}'", dk);
                    query = query + sql;
                }
                if (!string.IsNullOrEmpty(ik))
                {
                    string sql = string.Format(" and ItemKey='{0}'", ik);
                    query = query + sql;
                }
                if (!string.IsNullOrEmpty(iv))
                {
                    string sql = string.Format(" and ItemValue='{0}'", iv);
                    query = query + sql;
                }

                return conn.Query<DictionaryItemEntity>(query).ToList();

            }
        }

        /// <summary>
        /// 获取所有字典键值
        /// </summary>
        /// <returns></returns>
        public List<DictionaryItemEntity> GetAllDicKey()
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_DictionaryItems where status=1");
                return conn.Query<DictionaryItemEntity>(query).ToList();
            }
        }

        /// <summary>
        /// 根据键值key获取items
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<DictionaryItemEntity> GetDicItemsByDicKey(string key)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_DictionaryItems
                                            where DictionaryKey='{0}' and status=1", key);
                return conn.Query<DictionaryItemEntity>(query).ToList();
            }
        }

        public DictionaryItemEntity GetDicItemByKey(string itemId)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_DictionaryItems
                                            where ItemId='{0}'", itemId);
                return conn.Query<DictionaryItemEntity>(query).FirstOrDefault();
            }
        }


        public DictionaryItemEntity GetDicItemValueByKey(string ItemKey,string DicKey)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_DictionaryItems
                                            where DictionaryKey='{0}' and ItemKey='{1}'", DicKey, ItemKey);
                return conn.Query<DictionaryItemEntity>(query).FirstOrDefault();
            }
        }


        public int SaveDicItem(DictionaryItemEntity dicItem, string loginName)
        {
            if (string.IsNullOrEmpty(dicItem.ItemId))
            {
                var lResult = GetDicItemByKey(dicItem.DictionaryKey);
                if (lResult != null)
                {
                    dicItem.DictionaryLevel = lResult.DictionaryLevel + 1;
                }
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"INSERT INTO Train_DictionaryItems
                                                   (ItemId
                                                   ,ItemKey
                                                   ,ItemValue
                                                   ,Remark
                                                   ,OrderNum
                                                   ,Status
                                                   ,CreateTime
                                                   ,CreateUser
                                                   ,UpdateTime
                                                   ,UpdateUser
                                                   ,DictionaryKey
                                                   ,DictionaryLevel)
                                             VALUES
                                                   ('{0}'
                                                   ,'{1}'
                                                   ,'{2}'
                                                   ,'{3}'
                                                   ,'{4}'
                                                   ,'{5}'
                                                   ,getdate()
                                                   ,'{6}'
                                                   ,getdate()
                                                   ,'{7}'
                                                   ,'{8}'
                                                   ,'{9}')",
                        Guid.NewGuid().ToString(), dicItem.ItemKey,dicItem.ItemValue,dicItem.Remark,dicItem.OrderNum,
                        1, loginName, loginName, dicItem.DictionaryKey, dicItem.DictionaryLevel);
                    return conn.Execute(query);
                }
            }
            else
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@" UPDATE Train_DictionaryItems
                                                    SET ItemValue ='{0}'
                                                      ,Remark = '{1}'
                                                      ,OrderNum = {2}
                                                      ,UpdateTime = getdate()
                                                      ,UpdateUser = '{3}'
                                                    WHERE ItemId='{4}'",
                                               dicItem.ItemKey, dicItem.ItemValue, dicItem.Remark, dicItem.OrderNum, loginName, dicItem.ItemId);
                    return conn.Execute(query);
                }
            }
        }

        public int EnableItems(string ItemId, int status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_DictionaryItems set status={0}  where ItemId='{1}'", status, ItemId);
                return conn.Execute(query);
            }
        }

        public int DeleteItem(string ItemId)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"delete from Train_DictionaryItems  where ItemId='{0}'",  ItemId);
                return conn.Execute(query);
            }
        }
        #endregion

        #region 砍价
        public List<VMBargainConfig> GetBargainConfigsByCondition(string className)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.*,t1.ClassName from Train_BargainConfig t
                                                inner join Train_Class t1 on t.ClassId=t1.ClassId
                                            where t.status=1");
                if (!string.IsNullOrEmpty(className))
                {
                    string sql = string.Format(" and t1.ClassName='{0}'", className);
                    query = query + sql;
                }

                return conn.Query<VMBargainConfig>(query).ToList();

            }
        }

        public BargainConfigEntity GetBargainConfigByKey(string id)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_BargainConfig
                                            where BargainConfigId='{0}'", id);
                return conn.Query<BargainConfigEntity>(query).FirstOrDefault();
            }
        }


        public int SaveBargainConfig(BargainConfigEntity entity, string loginName)
        {
            if (string.IsNullOrEmpty(entity.BargainConfigId))
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"INSERT INTO Train_BargainConfig
                                                   (BargainConfigId
                                                   ,ClassId
                                                   ,FloorPrice
                                                   ,BargainTop
                                                   ,BargainFloor
                                                   ,Remark
                                                   ,Status
                                                   ,CreateTime
                                                   ,CreateUser
                                                   ,UpdateTime
                                                   ,UpdateUser
                                                   ,StartTime
                                                   ,EndTime)
                                             VALUES
                                                   ('{0}'
                                                   ,'{1}'
                                                   ,'{2}'
                                                   ,'{3}'
                                                   ,'{4}'
                                                   ,'{5}'
                                                   ,'{6}'
                                                   ,GETDATE()
                                                   ,'{7}'
                                                   ,GETDATE()
                                                   ,'{8}','{9}','{10}')",
                        Guid.NewGuid().ToString(), entity.ClassId, entity.FloorPrice, entity.BargainTop, entity.BargainFloor, entity.Remark, 1, loginName, loginName, entity.StartTime, entity.EndTime);
                    return conn.Execute(query);
                }
            }
            else
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"UPDATE Train_BargainConfig
                                                   SET ClassId ='{0}'
                                                      ,FloorPrice ='{1}'
                                                      ,BargainTop = '{2}'
                                                      ,BargainFloor ='{3}'
                                                      ,Remark = '{4}'
                                                      ,UpdateTime =GETDATE()
                                                      ,UpdateUser = '{5}'
                                                      ,StartTime='{6}'
                                                      ,EndTime='{7}'
                                                 WHERE BargainConfigId='{8}'", entity.ClassId, entity.FloorPrice, entity.BargainTop, entity.BargainFloor, entity.Remark, loginName,entity.StartTime, entity.EndTime, entity.BargainConfigId);
                    return conn.Execute(query);
                }
            }
        }

        public int EnableBargainConfig(string Id, int status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_BargainConfig set status={0}  where BargainConfigId='{1}'", status, Id);
                return conn.Execute(query);
            }
        }
        #endregion

        #region 团购
        public List<VMGroupBuyConfig> GetGroupBuyConfigsByCondition(string className)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.*,t1.ClassName from Train_GroupBuyConfig t
                                                inner join Train_Class t1 on t.ClassId=t1.ClassId
                                            where t.status=1");
                if (!string.IsNullOrEmpty(className))
                {
                    string sql = string.Format(" and t1.ClassName='{0}'", className);
                    query = query + sql;
                }

                return conn.Query<VMGroupBuyConfig>(query).ToList();

            }
        }

        public GroupBuyConfigEntity GetGroupBuyConfigByKey(string id)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_GroupBuyConfig
                                            where GroupBuyConfigId='{0}'", id);
                return conn.Query<GroupBuyConfigEntity>(query).FirstOrDefault();
            }
        }


        public int SaveGroupBuyConfig(GroupBuyConfigEntity entity, string loginName)
        {
            if (string.IsNullOrEmpty(entity.GroupBuyConfigId))
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"INSERT INTO Train_GroupBuyConfig
                                                   (GroupBuyConfigId
                                                   ,ClassId
                                                   ,GroupPrice
                                                   ,NeedCount
                                                   ,StartTime
                                                   ,EndTime
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
                                                   ,GETDATE()
                                                   ,'{8}'
                                                   ,GETDATE()
                                                   ,'{9}')",
                        Guid.NewGuid().ToString(), entity.ClassId, entity.GroupPrice, entity.NeedCount, entity.StartTime, entity.EndTime,entity.Remark, 1, loginName, loginName);
                    return conn.Execute(query);
                }
            }
            else
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"UPDATE Train_GroupBuyConfig
                                                   SET ClassId ='{0}'
                                                      ,GroupPrice ='{1}'
                                                      ,NeedCount = '{2}'
                                                      ,StartTime ='{3}'
                                                      ,EndTime ='{4}'
                                                      ,Remark = '{5}'
                                                      ,UpdateTime =GETDATE()
                                                      ,UpdateUser = '{6}'
                                                 WHERE GroupBuyConfigId='{7}'", entity.ClassId, entity.GroupPrice, entity.NeedCount, entity.StartTime, entity.EndTime, entity.Remark, loginName, entity.GroupBuyConfigId);
                    return conn.Execute(query);
                }
            }
        }

        public int EnableGroupBuyConfig(string Id, int status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_GroupBuyConfig set status={0}  where GroupBuyConfigId='{1}'", status, Id);
                return conn.Execute(query);
            }
        }
        #endregion

        #region 助力
        public List<VMHelpConfig> GetHelpConfigsByCondition(string className)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.*,t1.ClassName from Train_HelpConfig t
                                                inner join Train_Class t1 on t.ClassId=t1.ClassId
                                            where t.status=1");
                if (!string.IsNullOrEmpty(className))
                {
                    string sql = string.Format(" and t1.ClassName='{0}'", className);
                    query = query + sql;
                }

                return conn.Query<VMHelpConfig>(query).ToList();

            }
        }

        public HelpConfigEntity GetHelpConfigByKey(string id)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_HelpConfig
                                            where HelpConfigId='{0}'", id);
                return conn.Query<HelpConfigEntity>(query).FirstOrDefault();
            }
        }


        public int SaveHelpConfig(HelpConfigEntity entity, string loginName)
        {
            if (string.IsNullOrEmpty(entity.HelpConfigId))
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"INSERT INTO Train_HelpConfig
                                                   (HelpConfigId
                                                   ,ClassId
                                                   ,HelpCount
                                                   ,HelpPrice
                                                   ,StartTime
                                                   ,EndTime
                                                   ,Remark
                                                   ,Status
                                                   ,CreateTime
                                                   ,CreateUser
                                                   ,UpdateTime
                                                   ,UpdateUser
                                                   ,ImageUrl)
                                             VALUES
                                                   ('{0}'
                                                   ,'{1}'
                                                   ,'{2}'
                                                   ,'{3}'
                                                   ,'{4}'
                                                   ,'{5}'
                                                   ,'{6}'
                                                   ,'{7}'
                                                   ,GETDATE()
                                                   ,'{8}'
                                                   ,GETDATE()
                                                   ,'{9}'
                                                   ,'{10}')",
                        Guid.NewGuid().ToString(), entity.ClassId, entity.HelpCount, entity.HelpPrice, entity.StartTime, entity.EndTime, entity.Remark, 1, loginName, loginName,entity.ImageUrl);
                    return conn.Execute(query);
                }
            }
            else
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"UPDATE Train_HelpConfig
                                                   SET ClassId ='{0}'
                                                      ,HelpCount ='{1}'
                                                      ,HelpPrice = '{2}'
                                                      ,StartTime ='{3}'
                                                      ,EndTime ='{4}'
                                                      ,Remark = '{5}'
                                                      ,UpdateTime =GETDATE()
                                                      ,UpdateUser = '{6}',ImageUrl='{7}'
                                                 WHERE HelpConfigId='{8}'", entity.ClassId, entity.HelpCount, entity.HelpPrice, entity.StartTime, entity.EndTime, entity.Remark, loginName,entity.ImageUrl, entity.HelpConfigId);
                    return conn.Execute(query);
                }
            }
        }

        public int EnableHelpConfig(string Id, int status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_HelpConfig set status={0}  where HelpConfigId='{1}'", status, Id);
                return conn.Execute(query);
            }
        }
        #endregion

        #region 公益金
        public List<VMPublicFunds> GetPublicFundsByCondition(string className)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.*,t1.ClassName from Train_PublicFunds t
                                                inner join Train_Class t1 on t.ClassId=t1.ClassId
                                            where t.status=1");
                if (!string.IsNullOrEmpty(className))
                {
                    string sql = string.Format(" and t1.ClassName='{0}'", className);
                    query = query + sql;
                }

                return conn.Query<VMPublicFunds>(query).ToList();

            }
        }

        public PublicFundsEntity GetPublicFundsByKey(string id)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_PublicFunds
                                            where PublicFundsId='{0}'", id);
                return conn.Query<PublicFundsEntity>(query).FirstOrDefault();
            }
        }


        public int SavePublicFunds(PublicFundsEntity entity, string loginName)
        {
            if (string.IsNullOrEmpty(entity.PublicFundsId))
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"INSERT INTO Train_PublicFunds
                                                   (PublicFundsId
                                                   ,ClassId
                                                   ,FundsPrice                                                  
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
                                                   ,GETDATE()
                                                   ,'{4}'
                                                   ,GETDATE()
                                                   ,'{5}')",
                        Guid.NewGuid().ToString(), entity.ClassId, entity.FundsPrice, 1, loginName, loginName);
                    return conn.Execute(query);
                }
            }
            else
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"UPDATE Train_PublicFunds
                                                   SET ClassId ='{0}'
                                                      ,FundsPrice ='{1}'                                                      
                                                      ,UpdateTime =GETDATE()
                                                      ,UpdateUser = '{2}'
                                                 WHERE PublicFundsId='{3}'", entity.ClassId, entity.FundsPrice,loginName, entity.PublicFundsId);
                    return conn.Execute(query);
                }
            }
        }

        public int EnablePublicFunds(string Id, int status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_PublicFunds set status={0}  where PublicFundsId='{1}'", status, Id);
                return conn.Execute(query);
            }
        }
        #endregion

        #region 首页管理
        public List<IndexImageEntity> GetIndexImagesByCondition(string KeyName)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.*
                                               from Train_IndexImage t
                                               where t.status=1");
                if (!string.IsNullOrEmpty(KeyName))
                {
                    string sql = string.Format(" and t.ImageTitle like '%{0}%'", KeyName);
                    query = query + sql;
                }

                return conn.Query<IndexImageEntity>(query).ToList();

            }
        }


        /// <summary>
        /// 获取所有首页图片
        /// </summary>
        /// <returns></returns>
        public List<IndexImageEntity> GetAllIndexImages()
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_IndexImage where status=1");
                return conn.Query<IndexImageEntity>(query).ToList();
            }
        }

        public IndexImageEntity GetIndexImageByKey(string id)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_IndexImage
                                            where ImageId='{0}'", id);
                return conn.Query<IndexImageEntity>(query).FirstOrDefault();
            }
        }


        public int SaveIndexImage(IndexImageEntity indexImage, string loginName)
        {
            if (string.IsNullOrEmpty(indexImage.ImageId))
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"INSERT INTO Train_IndexImage
                                                   (ImageId
                                                   ,ImageTitle
                                                   ,ImageUrl
                                                   ,ImageLink
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
                                                   ,getdate()
                                                   ,'{6}'
                                                   ,getdate()
                                                   ,'{7}')",
                        Guid.NewGuid().ToString(), indexImage.ImageTitle,indexImage.ImageUrl,indexImage.ImageLink,
                        indexImage.Remark,1,loginName,loginName);
                    return conn.Execute(query);
                }
            }
            else
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@" UPDATE Train_IndexImage
                                                    SET ImageTitle ='{0}'
                                                      ,ImageUrl ='{1}'
                                                      ,ImageLink = '{2}'
                                                      ,Remark = '{3}'                                                      
                                                      ,UpdateTime = getdate()
                                                      ,UpdateUser = '{4}'
                                                    WHERE ImageId='{5}'",
                                              indexImage.ImageTitle, indexImage.ImageUrl, indexImage.ImageLink, indexImage.Remark, loginName, indexImage.ImageId);
                    return conn.Execute(query);
                }
            }
        }

        public int EnableImage(string ImageId, int status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_IndexImage set status={0}  where ImageId='{1}'", status, ImageId);
                return conn.Execute(query);
            }
        }
        #endregion

        #region 文章管理
        public List<ArticleEntity> GetArticlesByCondition(string KeyName)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.*
                                               from Train_Article t
                                               where t.status=1");
                if (!string.IsNullOrEmpty(KeyName))
                {
                    string sql = string.Format(" and t.ArticleTitl like '%{0}%'", KeyName);
                    query = query + sql;
                }

                return conn.Query<ArticleEntity>(query).ToList();

            }
        }


        /// <summary>
        /// 获取所有文章
        /// </summary>
        /// <returns></returns>
        public List<ArticleEntity> GetAllArticles()
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_Article where status=1");
                return conn.Query<ArticleEntity>(query).ToList();
            }
        }

        /// <summary>
        /// 获取所有文章
        /// </summary>
        /// <returns></returns>
        public List<VMWxArticle> GetAllArticlesByType(string type)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.*,isnull(tt.cc,0) as ClickCount from Train_Article t
                                               left join (select count(1) as cc,t1.ObjectId from Train_ClickCount t1 group by  t1.ObjectId) tt on t.ArticleId=tt.ObjectId
                                               where t.status=1 and t.ArticleType='{0}' order by t.createTime desc", type);
                return conn.Query<VMWxArticle>(query).ToList();
            }
        }

        public ArticleEntity GetArticleByKey(string id)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_Article
                                            where ArticleId='{0}'", id);
                return conn.Query<ArticleEntity>(query).FirstOrDefault();
            }
        }


        public int SaveArticle(ArticleEntity Article, string loginName)
        {
            if (string.IsNullOrEmpty(Article.ArticleId))
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"INSERT INTO Train_Article
                                                   (ArticleId
                                                   ,ArticleType
                                                   ,ArticleTitle
                                                   ,ImageUrl
                                                   ,ImageLink
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
                                                   ,getdate()
                                                   ,'{7}'
                                                   ,getdate()
                                                   ,'{8}')",
                        Guid.NewGuid().ToString(), Article.ArticleType,Article.ArticleTitle, Article.ImageUrl, Article.ImageLink,
                        Article.Remark, 1, loginName, loginName);
                    return conn.Execute(query);
                }
            }
            else
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@" UPDATE Train_Article
                                                    SET ArticleType='{0}'
                                                      ,ArticleTitle ='{1}'
                                                      ,ImageUrl ='{2}'
                                                      ,ImageLink = '{3}'
                                                      ,Remark = '{4}'                                                      
                                                      ,UpdateTime = getdate()
                                                      ,UpdateUser = '{5}'
                                                    WHERE ArticleId='{6}'",
                                              Article.ArticleType, Article.ArticleTitle, Article.ImageUrl, Article.ImageLink, Article.Remark, loginName, Article.ArticleId);
                    return conn.Execute(query);
                }
            }
        }

        public int EnableArticle(string ArticleId, int status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_Article set status={0}  where ArticleId='{1}'", status, ArticleId);
                return conn.Execute(query);
            }
        }
        #endregion

        #region 点击量
        public int SaveClickCount(string type, string id,string openid)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"INSERT INTO Train_ClickCount
                                                   (ClickId
                                                   ,ObjectId
                                                   ,ObjectType
                                                   ,OpenId                                                
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
                                                   ,getdate()
                                                   ,'system'
                                                   ,getdate()
                                                   ,'system')",
                    Guid.NewGuid().ToString(), id, type, openid,1);
                return conn.Execute(query);
            }
        }
        #endregion

        #region 上传中心
        public List<VMUpLoad> GetUpLoadCenterListByCondition(string title)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select t.*,t.UpLoadUrl as ShowUrl from Train_UpLoadCenter t
                                            where t.status=1");
                if (!string.IsNullOrEmpty(title))
                {
                    string sql = string.Format(" and t.UpLoadTitle='{0}'", title);
                    query = query + sql;
                }

                return conn.Query<VMUpLoad>(query).ToList();

            }
        }

        public UpLoadCenterEntity GetUpLoadCenterByKey(string id)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Read))
            {
                string query = string.Format(@"select * from Train_UpLoadCenter
                                            where UpLoadId='{0}'", id);
                return conn.Query<UpLoadCenterEntity>(query).FirstOrDefault();
            }
        }


        public int SaveUpLoadCenter(UpLoadCenterEntity entity, string loginName)
        {
            if (string.IsNullOrEmpty(entity.UpLoadId))
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"INSERT INTO Train_UpLoadCenter
                                                   (UpLoadId
                                                   ,UpLoadTitle
                                                   ,UpLoadUrl
                                                   ,UpLoadType                                                   
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
                                                   ,getdate()
                                                   ,'{5}'
                                                   ,getdate()
                                                   ,'{6}')",
                        Guid.NewGuid().ToString(), entity.UpLoadTitle, entity.UpLoadUrl, entity.UpLoadType, 1, loginName, loginName);
                    return conn.Execute(query);
                }
            }
            else
            {
                using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
                {
                    string query = string.Format(@"UPDATE Train_UpLoadCenter
                                                   SET UpLoadTitle = '{0}'
                                                      ,UpLoadUrl='{1}'                                                      
                                                      ,UpdateTime = getdate()
                                                      ,UpdateUser = '{2}'
                                                 WHERE UpLoadId='{3}'",
                                               entity.UpLoadTitle,entity.UpLoadUrl, loginName, entity.UpLoadId);
                    return conn.Execute(query);
                }
            }
        }

        public int EnableUpLoadCenter(string id, int status)
        {
            using (IDbConnection conn = DBContext.GetConnection(DataBaseName.AccountTrianDB, ReadOrWriteDB.Write))
            {
                string query = string.Format(@"update Train_UpLoadCenter set status={0}  where UpLoadId='{1}'", status, id);
                return conn.Execute(query);
            }
        }
        #endregion
    }
}
