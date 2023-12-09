using Cardinals.Enums;
using UnityEngine;

namespace Cardinals.Board {
    public interface IBoardInputHandler {
        public bool IsMouseHover { get; }
		public bool IsMouseHoverUI { get; }
		public int HoveredIdx { get; }
		public UIMouseDetectorType HoveredMouseDetectorType { get; }

        void Init(IBoardBuilder builder);
        Vector3[] CreateMouseDetectors(int count);
    }
}