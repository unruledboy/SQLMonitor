#region Copyright ?2007 Rotem Sapir
/*
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages arising from the
 * use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not claim
 * that you wrote the original software. If you use this software in a product,
 * an acknowledgment in the product documentation is required, as shown here:
 *
 * Portions Copyright ?2007 Rotem Sapir
 *
 * 2. No substantial portion of the source code of this library may be redistributed
 * without the express written permission of the copyright holders, where
 * "substantial" is defined as enough code to be recognizably from this library.
*/
#endregion

using System.Drawing;

namespace Xnlab.SQLMon.Controls.Tree
{
    public class NodePos
    {
        public NodePos()
        {
            //throw new System.NotImplementedException();
        }
        private string _nodeId;

        public string NodeId
        {
            get { return _nodeId; }
            set { _nodeId = value; }
        }
        private RectangleF _nodePosition;

        public RectangleF NodePosition
        {
            get { return _nodePosition; }
            set { _nodePosition = value; }
        }
        
        
    }
}
