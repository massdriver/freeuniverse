﻿using FreeUniverse.Common.Arch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace FreeUniverse.Common.Unity.Solar
{
    [CustomEditor(typeof(DescriptorSolar))]
    public sealed class DescriptorSolarRootExporter : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Export"))
            {
                ArchSolar archSolar = ((DescriptorSolar)target).ToArchObject() as ArchSolar;
                File.WriteAllText(EditorUtility.SaveFilePanelInProject("Save as txt arch", archSolar.nickname, "txt", null), JsonConvert.SerializeObject(archSolar, Formatting.Indented));
                Debug.Log("Solar exported");
            }
        }
    }

    [FieldCopyTarget(typeof(ArchSolar))]
    public sealed class DescriptorSolar : EditableArchDescriptor
    {
        [FieldCopy]
        public List<ArchSolarComponent> components { get; set; }

        public override ArchObject ToArchObject()
        {
            // Process components
            {
                components = new List<ArchSolarComponent>();

                foreach (Transform tr in gameObject.transform)
                {
                    DescriptorSolarComponent component = tr.gameObject.GetComponent<DescriptorSolarComponent>();

                    if (component == null)
                        continue;

                    components.Add(component.ToArchObject() as ArchSolarComponent);
                }
            }

            return ArchObject.Convert<DescriptorSolar, ArchSolar>(this);
        }

    }
}
