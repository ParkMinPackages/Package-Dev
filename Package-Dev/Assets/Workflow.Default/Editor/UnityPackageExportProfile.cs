using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ParkMinPackages.PackageDev.WorkflowDefault.Editor
{
	public sealed class UnityPackageExportProfile : ScriptableObject
	{
		const string CreateMenuPath =
			"Assets/ParkMinPackages/Create/Unity Package Export Profile";

		[MenuItem(CreateMenuPath, priority = 30)]
		static void CreateProfile() {
			ProjectWindowUtil.CreateAsset(
				CreateInstance<UnityPackageExportProfile>(),
				"UnityPackageExportProfile.asset"
			);
		}

		public void Export() {
			string sourcePath = NormalizeAssetPath(_sourceFolderPath);
			if (!AssetDatabase.IsValidFolder(sourcePath)) {
				throw new InvalidOperationException(
					$"Source folder is not a valid Unity asset folder: {_sourceFolderPath}"
				);
			}

			string outputPath = ResolveOutputPath(_outputPackagePath);
			string outputDirectory = Path.GetDirectoryName(outputPath);
			if (string.IsNullOrEmpty(outputDirectory)) {
				throw new InvalidOperationException(
					$"Output package directory is invalid: {_outputPackagePath}"
				);
			}

			Directory.CreateDirectory(outputDirectory);
			if (File.Exists(outputPath)) {
				File.Delete(outputPath);
			}

			AssetDatabase.ExportPackage(
				sourcePath,
				outputPath,
				ExportPackageOptions.Recurse
			);
			AssetDatabase.Refresh();

			Debug.Log(
				$"Exported '{sourcePath}' to '{outputPath}'.",
				this
			);
		}

		public string SourceFolderPath
		{
			get { return _sourceFolderPath; }
			set { _sourceFolderPath = value; }
		}

		public string OutputPackagePath
		{
			get { return _outputPackagePath; }
			set { _outputPackagePath = value; }
		}

		[SerializeField] string _sourceFolderPath = "Assets";
		[SerializeField] string _outputPackagePath = "Export.unitypackage";

		static string NormalizeAssetPath(string path) {
			return (path ?? string.Empty).Trim().Replace('\\', '/').TrimEnd('/');
		}

		static string ResolveOutputPath(string path) {
			string normalizedPath = (path ?? string.Empty).Trim();
			if (string.IsNullOrEmpty(normalizedPath)) {
				throw new InvalidOperationException(
					"Output package path cannot be empty."
				);
			}

			if (!normalizedPath.EndsWith(
				    ".unitypackage",
				    StringComparison.OrdinalIgnoreCase)) {
				normalizedPath += ".unitypackage";
			}

			if (!Path.IsPathRooted(normalizedPath)) {
				normalizedPath = Path.Combine(
					UnityPackageExportProfile.GetProjectPath(),
					normalizedPath
				);
			}

			return Path.GetFullPath(normalizedPath);
		}

		internal static string GetProjectPath() {
			DirectoryInfo projectDirectory =
				Directory.GetParent(Application.dataPath);
			if (projectDirectory == null) {
				throw new InvalidOperationException(
					"Unable to determine the current Unity project path."
				);
			}

			return projectDirectory.FullName;
		}
	}

	[CustomEditor(typeof(UnityPackageExportProfile))]
	public sealed class UnityPackageExportProfileEditor : UnityEditor.Editor
	{
		SerializedProperty _sourceFolderPath;
		SerializedProperty _outputPackagePath;

		void OnEnable() {
			_sourceFolderPath = serializedObject.FindProperty("_sourceFolderPath");
			_outputPackagePath =
				serializedObject.FindProperty("_outputPackagePath");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			EditorGUILayout.PropertyField(
				_sourceFolderPath,
				new GUIContent("Source Folder Path")
			);
			if (GUILayout.Button("Select Source Folder")) {
				SelectSourceFolder();
			}

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(
				_outputPackagePath,
				new GUIContent("Output Package Path")
			);
			if (GUILayout.Button("Select Output Package")) {
				SelectOutputPackage();
			}

			serializedObject.ApplyModifiedProperties();

			EditorGUILayout.Space();
			if (GUILayout.Button("Export", GUILayout.Height(28f))) {
				try {
					((UnityPackageExportProfile)target).Export();
				}
				catch (Exception exception) {
					Debug.LogException(exception, target);
				}
			}
		}

		void SelectSourceFolder() {
			string selectedPath = EditorUtility.OpenFolderPanel(
				"Select Source Folder",
				Application.dataPath,
				string.Empty
			);
			if (string.IsNullOrEmpty(selectedPath)) return;

			string projectPath = Path.GetFullPath(
				UnityPackageExportProfile.GetProjectPath()
			).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
			string selectedFullPath = Path.GetFullPath(selectedPath);
			string projectPathPrefix = projectPath + Path.DirectorySeparatorChar;

			if (!selectedFullPath.StartsWith(
				    projectPathPrefix,
				    StringComparison.OrdinalIgnoreCase)) {
				EditorUtility.DisplayDialog(
					"Invalid Source Folder",
					"The source folder must be inside the current Unity project.",
					"OK"
				);
				return;
			}

			string relativePath = selectedFullPath
				.Substring(projectPathPrefix.Length)
				.Replace('\\', '/');
			_sourceFolderPath.stringValue = relativePath;
			serializedObject.ApplyModifiedProperties();
		}

		void SelectOutputPackage() {
			string selectedPath = EditorUtility.SaveFilePanel(
				"Select Output Package",
				UnityPackageExportProfile.GetProjectPath(),
				"Export",
				"unitypackage"
			);
			if (string.IsNullOrEmpty(selectedPath)) return;

			_outputPackagePath.stringValue = selectedPath;
			serializedObject.ApplyModifiedProperties();
		}
	}
}
