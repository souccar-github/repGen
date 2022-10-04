#region

using System.Collections.Generic;
using System.Linq;
using HRIS.Domain.OrgChart.ValueObjects;
using Infrastructure.Entities;

#endregion

namespace Service.OrgChart
{
    public static class NodeHelper
    {
        #region Properties

        private static EntityServiceBase<Node> _service;

        public static EntityServiceBase<Node> Service
        {
            get { return _service ?? (_service = new EntityService<Node>()); }
        }

        #endregion

        public static void ReassignNodesToNewParent(int newParentNodeId, List<int> selectedNodesIds)
        {
            #region Get Selected Nodes

            var nodes = new List<Node>(selectedNodesIds.Count);
            nodes.AddRange(selectedNodesIds.Select(t => Service.GetById(t)));

            #endregion

            #region Get Destination Node (New Parent)

            Node destinationNode = Service.GetById(newParentNodeId);

            #endregion

            foreach (Node node in nodes)
            {
                node.Parent = destinationNode;
            }

            Service.Update(destinationNode);
        }

        public static void MoveNodePositionsToAnotherNode(int destination, int source)
        {
            #region Get Selected Nodes

            var destinationNode = Service.GetById(destination);
            var sourceNode = Service.GetById(source);

            #endregion

            #region Move Position To Destination Node

            if (sourceNode.Positions.Count > 0)
            {
                //todo check
                //foreach (var position in sourceNode.Positions)
                //{
                //    position.Node = destinationNode;
                //}
            }

            #endregion

            Service.Update(destinationNode);
        }

        public static void MoveSelectedPositionsBetweenTwoNodes(int destination, int source, List<int> selectedNodesIds)
        {
            #region Get Selected Nodes

            var destinationNode = Service.GetById(destination);
            var sourceNode = Service.GetById(source);

            #endregion

            #region Move Position To Destination Node

            if (sourceNode.Positions.Count > 0)
            {
                foreach (var position in sourceNode.Positions)
                {
                    foreach (int selectedNodesId in selectedNodesIds)
                    {
                        //todo check
                        //if (position.Id == selectedNodesId)
                        //{
                        //    position.Node = destinationNode;
                        //}
                    }
                }
            }

            #endregion

            Service.Update(destinationNode);
        }
    }
}