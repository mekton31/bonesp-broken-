using ClickableTransparentOverlay;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace bonesp
{
    public class Renderer : Overlay
    {
        public Vector2 overlaySize = new Vector2(1920, 1080);
        Vector2 windowLocation = new Vector2(0, 0); //if wi,ndows full scrren xy= 0
        public List<Entity> entitiesCopy = new List<Entity>();
        public Entity localPlayerCopy = new Entity();
        ImDrawListPtr drawList;
        public bool esp = false;
        Vector4 teamColor = new Vector4(1, 1, 1, 1); //white
        Vector4 enemyColor = new Vector4(1,1, 1, 1);

        float boneThickness = 4;

        protected override void Render()
        {
            ImGui.Begin("test");
            ImGui.Checkbox("esp", ref esp);
            ImGui.SliderFloat("Bone thickness", ref boneThickness, 4, 500);
            //clor scheme
            if (ImGui.CollapsingHeader("team color")) //team
                ImGui.ColorPicker4("##teamcolor", ref teamColor);
            //enmy
            if(ImGui.CollapsingHeader("enemy color")) //enemy
                ImGui.ColorPicker4("##enemycolor", ref enemyColor);
            if (esp)
            {
                DrawOverlay();
                DrawSkeletons();
            }
            ImGui.End();
        }

        void DrawSkeletons()
        {
           if (entitiesCopy.Count == 0 || entitiesCopy == null)
                return;
           
           List<Entity> tempEntities = new List<Entity>(entitiesCopy).ToList(); //make anotehr copy

            drawList = ImGui.GetWindowDrawList();
            uint uintColor;

            //loop thrpuhhj bones and draw
            foreach (Entity entity in tempEntities) 
            {
              if (entity == null) continue;
                uintColor = localPlayerCopy.team == entity.team ? ImGui.ColorConvertFloat4ToU32(teamColor) : ImGui.ColorConvertFloat4ToU32(enemyColor);
             //check that enittis on scrren
                if (entity.bones2d[2].X > 0 && entity.bones2d[2].Y > 0 && entity.bones2d[2].X < overlaySize.X && entity.bones2d[2].Y < overlaySize.Y)
                {
                    float currentBoneThickness = boneThickness / entity.distance; // noy perfect but sometingh
                                                                                  //draw lines betwen bones
                    drawList.AddLine(entity.bones2d[1], entity.bones2d[2], uintColor, currentBoneThickness); //neck to head
                    drawList.AddLine(entity.bones2d[1], entity.bones2d[3], uintColor, currentBoneThickness); // neck to leftshoudkler
                    drawList.AddLine(entity.bones2d[1], entity.bones2d[6], uintColor, currentBoneThickness);
                    drawList.AddLine(entity.bones2d[3], entity.bones2d[4], uintColor, currentBoneThickness);
                    drawList.AddLine(entity.bones2d[6], entity.bones2d[7], uintColor, currentBoneThickness);
                    drawList.AddLine(entity.bones2d[4], entity.bones2d[5], uintColor, currentBoneThickness);
                    drawList.AddLine(entity.bones2d[7], entity.bones2d[8], uintColor, currentBoneThickness);
                    drawList.AddLine(entity.bones2d[1], entity.bones2d[0], uintColor, currentBoneThickness);
                    drawList.AddLine(entity.bones2d[0], entity.bones2d[9], uintColor, currentBoneThickness);
                    drawList.AddLine(entity.bones2d[0], entity.bones2d[11], uintColor, currentBoneThickness);
                    drawList.AddLine(entity.bones2d[9], entity.bones2d[10], uintColor, currentBoneThickness);
                    drawList.AddLine(entity.bones2d[11], entity.bones2d[12], uintColor, currentBoneThickness);
                    drawList.AddCircle(entity.bones2d[2], 3 + currentBoneThickness, uintColor);
                }
            
            }

        }


        void DrawOverlay()
        {
            ImGui.SetNextWindowSize(overlaySize);
            ImGui.SetNextWindowPos(windowLocation);
            ImGui.Begin("overlay", ImGuiWindowFlags.NoDecoration
                | ImGuiWindowFlags.NoBackground
                | ImGuiWindowFlags.NoBringToFrontOnFocus
                | ImGuiWindowFlags.NoMove
                | ImGuiWindowFlags.NoInputs
                | ImGuiWindowFlags.NoCollapse
                | ImGuiWindowFlags.NoScrollbar
                | ImGuiWindowFlags.NoScrollWithMouse);
        }
    }
}
