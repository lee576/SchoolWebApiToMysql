namespace Models.ViewModels
{
    public class FindOrderStateServiceModel
    {
        /// <summary>
        /// 应用id
        /// </summary>
        public long appId { set; get; }
        /// <summary>
        /// 学生id
        /// </summary>
        public long userId { set; get; }
        /// <summary>
        /// 学校或部门id
        /// </summary>
        public long deptId { set; get; }
        /// <summary>
        /// 消费订单id
        /// </summary>
        public string orderId { set; get; }
    }
}
