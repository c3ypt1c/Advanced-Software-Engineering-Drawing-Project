﻿namespace Advanced_Software_Engineering.Verbs.DrawingVerbs {

    /// <summary>
    /// Fill IVerb class
    /// </summary>
    public class Fill : IVerb {
        private readonly Drawer drawer;
        private readonly bool enable;

        /// <summary>
        /// Fill enables or disables the fill insider drawer
        /// </summary>
        /// <param name="drawer">drawer</param>
        /// <param name="enable">Enable fill?</param>
        public Fill(Drawer drawer, bool enable) {
            this.drawer = drawer;
            this.enable = enable;
        }

        /// <summary>
        /// Sets the fill status
        /// </summary>
        public void ExecuteVerb() {
            if (enable) drawer.EnableFill();
            else drawer.DisableFill();
        }

        /// <summary>
        /// Describes what the command will do
        /// </summary>
        /// <returns>Description of what the IVerb will do</returns>
        public string GetDescription() {
            if (enable) return "Enable fill";
            else return "Disable fill";
        }
    }
}