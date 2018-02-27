using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using SimpleFramework.Utils;

public class EditorUtils {
    public static bool FilterFile(string path) {
		if (path.EndsWith(".meta") || path.EndsWith(".DS_Store") || path.Contains(".svn") || path.EndsWith(".manifest"))
			return false;
		return true;
	}
}