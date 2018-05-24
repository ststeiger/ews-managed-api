
namespace libldapsync.Data
{

    public abstract class DbEntity
    {
        protected AnySQL SQL;

        public DbEntity(AnySQL sql)
        {
            this.SQL = sql;
        }

    }


    public class DbUser
        : DbEntity
    {

        public DbUser(AnySQL sql)
        : base(sql)
        { }


        public virtual bool Exists(int userId)
        {
            return false;
        }


        public virtual bool Exists(string userName)
        {
            return false;
        }


        public virtual bool ExistsOrDeleted(int userId)
        {
            return false;
        }


        public virtual bool ExistsOrDeleted(string userName)
        {
            return false;
        }


        public virtual void Insert()
        { }

        public virtual void Update()
        { }


        public static DbUser Get(AnySQL sql, int userId)
        {
            DbUser dbUser = new DbUser(sql);
            return dbUser;
        }


        public static DbUser Get(AnySQL sql, string userName)
        {
            DbUser dbUser = new DbUser(sql);
            return dbUser;
        }


    } // End Class cUser 




    public class DbGroup
        : DbEntity
    {

        public DbGroup(AnySQL sql)
        : base(sql)
        { }


        public virtual System.Collections.Generic.List<DbGroup> GetLdapGroups()
        {
            return null;
        }


        public virtual bool Exists(int groupId)
        {
            return false;
        }


        public virtual bool Exists(string groupName)
        {
            return false;
        }


        public virtual bool ExistsOrDeleted(int groupId)
        {
            return false;
        }


        public virtual bool ExistsOrDeleted(string groupName)
        {
            return false;
        }


        public virtual void Insert()
        { }



        public virtual void Update(int groupId)
        { }

        public virtual void Update(string groupName)
        { }


        public static DbGroup Get(AnySQL sql, int groupId)
        {
            DbGroup dbGroup = new DbGroup(sql);
            return dbGroup;
        }


        public static DbGroup Get(AnySQL sql, string groupName)
        {
            DbGroup dbGroup = new DbGroup(sql);
            return dbGroup;
        }


    } // End Class cGroup 


    public class DbUserGroupMapping
        : DbEntity
    {

        public DbUserGroupMapping(AnySQL sql)
        : base(sql)
        { }

        public virtual bool Exists(int userId, int groupId)
        {
            return false;
        }

        public virtual bool Exists(string userName, string groupName)
        {
            return false;
        }

        public virtual bool Exists(int userId, string groupName)
        {
            return false;
        }

        public virtual bool Exists(string userName, int groupId)
        {
            return false;
        }

        public virtual void Insert()
        { }

        public virtual void Update()
        { }

        public static DbUserGroupMapping Get(AnySQL sql, int userId, int groupId)
        {
            DbUserGroupMapping groupMapping = new DbUserGroupMapping(sql);
            return groupMapping;
        }

        public static DbUserGroupMapping Get(AnySQL sql, string userName, string groupName)
        {
            DbUserGroupMapping groupMapping = new DbUserGroupMapping(sql);
            return groupMapping;
        }

        public static DbUserGroupMapping Get(AnySQL sql, int userId, string groupName)
        {
            DbUserGroupMapping groupMapping = new DbUserGroupMapping(sql);
            return groupMapping;
        }

        public static DbUserGroupMapping Get(AnySQL sql, string userName, int groupId)
        {
            DbUserGroupMapping groupMapping = new DbUserGroupMapping(sql);
            return groupMapping;
        }

    } // End Class cUserGroupMapping 



    public class LdapUser
    {

        public string cn;
        public string distinguishedName;
        public string sAMAccountName;
        public string userPrincipalName;
        public string displayName;
        public string givenName;
        public string sn;
        public string mail;
        public string mailNickname;
        public System.Collections.Generic.List<string> memberOf;
        public string homeDirectory;
        public string msExchUserCulture;


        public LdapUser()
        {
            this.memberOf = new System.Collections.Generic.List<string>();
        }

        public virtual void foo()
        { }


        public static LdapUser Get()
        {
            var user = new LdapUser();
            return user;
        }


        public static LdapUser FromEntry(Novell.Directory.Ldap.LdapEntry entry)
        {
            LdapUser user = new LdapUser();

            string[] attrs = new string[] { "cn", "distinguishedName", "sAMAccountName", "userPrincipalName"
                , "displayName", "givenName", "sn", "mail", "mailNickname"
                , "memberOf", "homeDirectory", "msExchUserCulture" };

            System.Reflection.FieldInfo[] fis = typeof(LdapUser).GetFields();

            foreach (System.Reflection.FieldInfo fi in fis)
            {
                string fieldName = fi.Name;

                int ind = System.Array.FindIndex(attrs, x => string.Equals(x, fieldName, System.StringComparison.InvariantCultureIgnoreCase));
                if (ind == -1)
                    continue;

                string attributeName = attrs[ind];
                Novell.Directory.Ldap.LdapAttribute att = entry.getAttribute(attributeName);

                if (att == null)
                    continue;

                if (fi.FieldType.IsGenericType)
                {
                    fi.FieldType.GetMethod("AddRange").Invoke(fi.GetValue(user), new object[] { att.StringValueArray });
                }
                else
                    fi.SetValue(user, att.StringValue);
            } // Next fi 

            return user;
        } // End Function FromEntry 


    } // End Class LdapUser 


    public class LdapGroup
    {

        public string cn;
        public string sAMAccountName;
        public string distinguishedName;
        public System.Collections.Generic.List<string> member;


        public LdapGroup()
        {
            this.member = new System.Collections.Generic.List<string>();
        }


        public virtual System.Collections.Generic.List<LdapUser> GetAllMemberUsers()
        {
            return null;
        }


        public static LdapGroup FromEntry(Novell.Directory.Ldap.LdapEntry entry)
        {
            LdapGroup group = new LdapGroup();

            string[] attrs = new string[] { "cn", "sAMAccountName", "distinguishedName", "member" };

            System.Reflection.FieldInfo[] fis = typeof(LdapGroup).GetFields();

            foreach (System.Reflection.FieldInfo fi in fis)
            {
                string fieldName = fi.Name;

                int ind = System.Array.FindIndex(attrs, x => string.Equals(x, fieldName, System.StringComparison.InvariantCultureIgnoreCase));
                if (ind == -1)
                    continue;

                string attributeName = attrs[ind];
                Novell.Directory.Ldap.LdapAttribute att = entry.getAttribute(attributeName);

                if (att == null)
                    continue;

                if(fi.FieldType.IsGenericType)
                {
                    fi.FieldType.GetMethod("AddRange").Invoke(fi.GetValue(group), new object[] { att.StringValueArray });
                }
                else
                    fi.SetValue(group, att.StringValue);
            } // Next fi 

            return group;
        } // End Function FromEntry 

    } // End Class LdapGroup 


    public class ActiveDirectory
    {

        public string URL;
        public int Port;



        public string UserName;
        public string Password;


        public ActiveDirectory()
        { }



        public virtual System.Collections.Generic.List<LdapGroup> GetAllGroups()
        {
            return null;
        }


        public virtual System.Collections.Generic.List<LdapGroup> GetGroups(System.Collections.Generic.List<DbGroup> ls)
        {
            return null;
        }


        public virtual bool IsUserInGroup(string user, string group)
        {
            return true;
        }


    } // End Class ActiveDirectory
    

    public class DBAL
    {

        protected AnySQL SQL;
        public DbUser User;
        public DbGroup Group;
        public DbUserGroupMapping UserGroupMapping;


        public DBAL()
        {
            this.SQL = AnySQL.CreateInstance();
            this.User = new DbUser(this.SQL);
            this.Group = new DbGroup(this.SQL);
            this.UserGroupMapping = new DbUserGroupMapping(this.SQL);
        }

    }


}
