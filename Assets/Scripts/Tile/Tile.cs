using UnityEngine;

namespace RoadTrip
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] 
        private int _tileID ;
        
        [SerializeField] 
        private int _tileEntryID;
        
        [SerializeField]
        private int _tileExitID;
        
        public int GetTileID() => _tileID;
        
        public int GetTileEntryID() => _tileEntryID;
        
        public int GetTileExitID() => _tileExitID;
        
        Tile()
        {
            _tileID =  GetHashCode();
            _tileEntryID = 0;
            _tileExitID = 0;
        }
    }
}