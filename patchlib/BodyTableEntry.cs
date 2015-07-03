using System;
using System.Collections.Generic;
using System.Text;


namespace CUODesktop.PatchLib
{
    public class BodyTableEntry
    {
        public int m_OldID;
        public int m_NewID;
        public int m_NewHue;

        public BodyTableEntry(int oldID, int newID, int newHue)
        {
            m_OldID = oldID;
            m_NewID = newID;
            m_NewHue = newHue;
        }
    }
}
