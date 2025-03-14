
namespace SiliconeHeart.Building
{
    public class Building
    {
        public string DataId { get; private set; }
        public float PositionX;
        public float PositionY;

        public Building(string dataId, float positionX, float positionY)
        {
            DataId = dataId;
            PositionX = positionX;
            PositionY = positionY;
        }
    }
}