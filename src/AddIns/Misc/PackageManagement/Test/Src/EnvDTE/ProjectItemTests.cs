﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections;
using System.Collections.Generic;

using ICSharpCode.Core;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using ICSharpCode.PackageManagement.EnvDTE;
using ICSharpCode.SharpDevelop;
using ICSharpCode.SharpDevelop.Dom;
using NUnit.Framework;
using PackageManagement.Tests.Helpers;
using Rhino.Mocks;
using DTE = ICSharpCode.PackageManagement.EnvDTE;

namespace PackageManagement.Tests.EnvDTE
{
	[TestFixture]
	public class ProjectItemTests : CodeModelTestBase
	{
		TestableDTEProject dteProject;
		ProjectItems projectItems;
		TestableProject msbuildProject;
		FakeFileService fakeFileService;
		
		void CreateProjectItems(string fileName = @"d:\projects\MyProject\MyProject.csproj")
		{
			dteProject = new TestableDTEProject();
			msbuildProject = dteProject.TestableProject;
			msbuildProject.FileName = new FileName(fileName);
			projectItems = (ProjectItems)dteProject.ProjectItems;
			fakeFileService = dteProject.FakeFileService;
		}
		
		void OpenSavedFileInSharpDevelop(string fileName)
		{
			OpenFileInSharpDevelop(fileName, dirty: false);
		}
		
		void OpenUnsavedFileInSharpDevelop(string fileName)
		{
			OpenFileInSharpDevelop(fileName, dirty: true);
		}
		
		IViewContent OpenFileInSharpDevelop(string fileName, bool dirty)
		{
			IViewContent view = MockRepository.GenerateStub<IViewContent>();
			view.Stub(v => v.IsDirty).Return(dirty);
			fakeFileService.AddOpenView(view, fileName);
			return view;
		}
		
		void AddCompilationUnit()
		{
			fakeFileService.CompilationUnitToReturnFromGetCompilationUnit = projectContent.CreateCompilation();
		}
		
		[Test]
		public void ProjectItems_ProjectHasOneFileInsideSrcDirectory_ReturnsOneFileForSrcDirectory()
		{
			CreateProjectItems();
			msbuildProject.AddFile(@"src\program.cs");
			
			global::EnvDTE.ProjectItem directoryItem = projectItems.Item("src");
			global::EnvDTE.ProjectItems directoryProjectItems = directoryItem.ProjectItems;
			
			string[] expectedFiles = new string[] {
				"program.cs"
			};
			
			ProjectItemCollectionAssert.AreEqual(expectedFiles, directoryProjectItems);
		}
		
		[Test]
		public void ProjectItems_ProjectHasTestDirectoryInsideSrcDirectory_ReturnsTestDirectoryItemForSrcDirectory()
		{
			CreateProjectItems();
			msbuildProject.AddDirectory(@"src\test");
			
			global::EnvDTE.ProjectItem directoryItem = projectItems.Item("src");
			global::EnvDTE.ProjectItems directoryProjectItems = directoryItem.ProjectItems;
			var items = directoryProjectItems as IEnumerable;
			
			string[] expectedItems = new string[] {
				"test"
			};
			
			ProjectItemCollectionAssert.AreEqual(expectedItems, items);
		}
		
		[Test]
		public void ProjectItems_ProjectHasTwoFilesOneNotInSrcDirectory_ReturnsOneFileItemForSrcDirectory()
		{
			CreateProjectItems();
			msbuildProject.AddFile(@"src\test.cs");
			msbuildProject.AddFile("program.cs");
			
			global::EnvDTE.ProjectItem directoryItem = projectItems.Item("src");
			global::EnvDTE.ProjectItems directoryProjectItems = directoryItem.ProjectItems;
			var items = directoryProjectItems as IEnumerable;
			
			string[] expectedItems = new string[] {
				"test.cs"
			};
			
			ProjectItemCollectionAssert.AreEqual(expectedItems, items);
		}
		
		[Test]
		public void ProjectItems_ProjectHasOneFileInTestDirectoryTwoLevelsDeep_ReturnsOneFileItemForTestDirectory()
		{
			CreateProjectItems();
			msbuildProject.AddFile(@"src\test\test.cs");
		
			global::EnvDTE.ProjectItem directoryItem = projectItems.Item("src");
			global::EnvDTE.ProjectItem testDirectoryItem = directoryItem.ProjectItems.Item("test");
			global::EnvDTE.ProjectItems testDirectoryProjectItems = testDirectoryItem.ProjectItems;
			var items = testDirectoryProjectItems as IEnumerable;
			
			string[] expectedItems = new string[] {
				"test.cs"
			};
			
			ProjectItemCollectionAssert.AreEqual(expectedItems, items);
		}
		
		[Test]
		public void Kind_ProjectDirectory_ReturnsGuidForDirectory()
		{
			CreateProjectItems();
			msbuildProject.AddFile(@"src\program.cs");
		
			ProjectItem directoryItem = (ProjectItem)projectItems.Item("src");
			
			string kind = directoryItem.Kind;
			
			Assert.AreEqual(global::EnvDTE.Constants.vsProjectItemKindPhysicalFolder, kind);
		}
		
		[Test]
		public void Kind_ProjectFile_ReturnsGuidForFile()
		{
			CreateProjectItems();
			msbuildProject.AddFile(@"src\program.cs");
		
			global::EnvDTE.ProjectItem directoryItem = projectItems.Item("src");
			global::EnvDTE.ProjectItem fileItem = directoryItem.ProjectItems.Item("program.cs");
			
			string kind = fileItem.Kind;
			
			Assert.AreEqual(global::EnvDTE.Constants.vsProjectItemKindPhysicalFile, kind);
		}
		
		[Test]
		public void Delete_ProjectItemIsFile_FileIsDeleted()
		{
			CreateProjectItems(@"d:\projects\myproject\myproject.csproj");
			msbuildProject.AddFile(@"src\program.cs");
			global::EnvDTE.ProjectItem directoryItem = projectItems.Item("src");
			global::EnvDTE.ProjectItem fileItem = directoryItem.ProjectItems.Item("program.cs");
			
			fileItem.Delete();
			
			Assert.AreEqual(@"d:\projects\myproject\src\program.cs", dteProject.FakeFileService.PathPassedToRemoveFile);
		}
		
		[Test]
		public void Delete_ProjectItemIsFile_ProjectIsSaved()
		{
			CreateProjectItems(@"d:\projects\myproject\myproject.csproj");
			msbuildProject.AddFile(@"src\program.cs");
			global::EnvDTE.ProjectItem directoryItem = projectItems.Item("src");
			global::EnvDTE.ProjectItem fileItem = directoryItem.ProjectItems.Item("program.cs");
			
			fileItem.Delete();
			
			Assert.IsTrue(msbuildProject.IsSaved);
		}
		
		[Test]
		public void FileCodeModel_ProjectDirectory_ReturnsNull()
		{
			CreateProjectItems();
			msbuildProject.AddFile(@"src\program.cs");
		
			global::EnvDTE.ProjectItem directoryItem = projectItems.Item("src");
			
			global::EnvDTE.FileCodeModel2 fileCodeModel = directoryItem.FileCodeModel;
			
			Assert.IsNull(fileCodeModel);
		}
		
		[Test]
		public void FileCodeModel_ProjectFile_ReturnsFileCodeModel()
		{
			CreateProjectItems();
			AddCompilationUnit();
			msbuildProject.AddFile(@"src\program.cs");
		
			global::EnvDTE.ProjectItem directoryItem = projectItems.Item("src");
			global::EnvDTE.ProjectItem fileItem = directoryItem.ProjectItems.Item("program.cs");
			
			global::EnvDTE.FileCodeModel2 fileCodeModel = fileItem.FileCodeModel;
			
			Assert.IsNotNull(fileCodeModel);
		}
		
		[Test]
		public void FileCodeModel_GetCodeElementsFromFileCodeModelForProjectFile_FileServicePassedToFileCodeModel()
		{
			CreateProjectItems(@"d:\projects\MyProject\MyProject.csproj");
			AddCompilationUnit();
			msbuildProject.AddFile(@"src\program.cs");
			global::EnvDTE.ProjectItem directoryItem = projectItems.Item("src");
			global::EnvDTE.ProjectItem fileItem = directoryItem.ProjectItems.Item("program.cs");
			
			global::EnvDTE.CodeElements codeElements = fileItem.FileCodeModel.CodeElements;
			
			CodeNamespace codeNamespace = codeElements.FirstCodeNamespaceOrDefault();
			Assert.AreEqual(dteProject.TestableProject, fakeFileService.ProjectPassedToGetCompilationUnit);
			Assert.AreEqual(0, codeElements.Count);
		}
		
		[Test]
		public void Remove_FileItemInProject_FileItemRemovedFromProject()
		{
			CreateProjectItems();
			msbuildProject.AddFile(@"program.cs");
			
			global::EnvDTE.ProjectItem fileItem = projectItems.Item("program.cs");
			fileItem.Remove();
			
			Assert.AreEqual(0, msbuildProject.Items.Count);
		}
		
		[Test]
		public void Remove_FileItemInProject_ProjectIsSaved()
		{
			CreateProjectItems();
			msbuildProject.AddFile(@"program.cs");
			
			global::EnvDTE.ProjectItem fileItem = projectItems.Item("program.cs");
			fileItem.Remove();
			
			Assert.IsTrue(msbuildProject.IsSaved);
		}
		
		[Test]
		public void FileNames_IndexOneRequested_ReturnsFullPathToProjectItem()
		{
			CreateProjectItems(@"d:\projects\MyProject\MyProject.csproj");
			msbuildProject.AddFile(@"src\program.cs");
			global::EnvDTE.ProjectItem directoryItem = projectItems.Item("src");
			
			string fileName = directoryItem.get_FileNames(1);
			
			Assert.AreEqual(@"d:\projects\MyProject\src", fileName);
		}
		
		[Test]
		public void Document_ProjectItemNotOpenInSharpDevelop_ReturnsNull()
		{
			CreateProjectItems();
			msbuildProject.FileName = new FileName(@"d:\projects\MyProject\MyProject.csproj");
			msbuildProject.AddFile(@"program.cs");
			global::EnvDTE.ProjectItem item = projectItems.Item("program.cs");
			
			global::EnvDTE.Document document = item.Document;
			
			Assert.IsNull(document);
		}
		
		[Test]
		public void Document_ProjectItemOpenInSharpDevelop_ReturnsOpenDocumentForFile()
		{
			CreateProjectItems();
			msbuildProject.FileName = new FileName(@"d:\projects\MyProject\MyProject.csproj");
			msbuildProject.AddFile(@"program.cs");
			global::EnvDTE.ProjectItem item = projectItems.Item("program.cs");
			string projectItemFileName = @"d:\projects\MyProject\program.cs";
			OpenSavedFileInSharpDevelop(projectItemFileName);
			
			global::EnvDTE.Document document = item.Document;
			
			Assert.AreEqual(projectItemFileName, document.FullName);
		}
		
		[Test]
		public void Document_ProjectItemOpenInSharpDevelopAndIsSaved_ReturnsOpenDocumentThatIsSaved()
		{
			CreateProjectItems();
			msbuildProject.FileName = new FileName(@"d:\projects\MyProject\MyProject.csproj");
			msbuildProject.AddFile(@"program.cs");
			global::EnvDTE.ProjectItem item = projectItems.Item("program.cs");
			OpenSavedFileInSharpDevelop(@"d:\projects\MyProject\program.cs");
			
			global::EnvDTE.Document document = item.Document;
			
			Assert.IsTrue(document.Saved);
		}
		
		[Test]
		public void Document_ProjectItemOpenInSharpDevelopAndIsUnsaved_ReturnsOpenDocumentThatIsNotSaved()
		{
			CreateProjectItems();
			msbuildProject.FileName = new FileName(@"d:\projects\MyProject\MyProject.csproj");
			msbuildProject.AddFile(@"program.cs");
			global::EnvDTE.ProjectItem item = projectItems.Item("program.cs");
			OpenUnsavedFileInSharpDevelop(@"d:\projects\MyProject\program.cs");
			
			global::EnvDTE.Document document = item.Document;
			
			Assert.IsFalse(document.Saved);
		}
		
		[Test]
		public void Open_ViewKindIsCode_OpensFile()
		{
			CreateProjectItems();
			msbuildProject.FileName = new FileName(@"d:\projects\MyProject\MyProject.csproj");
			msbuildProject.AddFile(@"program.cs");
			global::EnvDTE.ProjectItem item = projectItems.Item("program.cs");
			
			global::EnvDTE.Window window = item.Open(global::EnvDTE.Constants.vsViewKindCode);
			
			Assert.AreEqual(@"d:\projects\MyProject\program.cs", fakeFileService.FileNamePassedToOpenFile);
		}
		
		[Test]
		public void ProjectItems_ProjectItemHasDependentFile_DependentFileNotAvailableFromProjectItems()
		{
			CreateProjectItems();
			msbuildProject.AddFile("MainForm.cs");
			msbuildProject.AddDependentFile("MainForm.Designer.cs", "MainForm.cs");
			
			global::EnvDTE.ProjectItems projectItems = dteProject.ProjectItems;
			
			string[] expectedFiles = new string[] {
				"MainForm.cs"
			};
			ProjectItemCollectionAssert.AreEqual(expectedFiles, projectItems);
		}
		
		[Test]
		public void ProjectItems_ProjectItemHasDependentFile_DependentFileNotAvailableFromProject()
		{
			CreateProjectItems();
			msbuildProject.AddFile("MainForm.cs");
			msbuildProject.AddDependentFile("MainForm.Designer.cs", "MainForm.cs");
			
			Assert.Throws<ArgumentException>(() => dteProject.ProjectItems.Item("MainForm.Designer.cs"));
		}
		
		[Test]
		public void ProjectItems_ProjectItemHasDependentFile_ReturnsDependentFile()
		{
			CreateProjectItems();
			msbuildProject.AddFile("MainForm.cs");
			msbuildProject.AddDependentFile("MainForm.Designer.cs", "MainForm.cs");
			global::EnvDTE.ProjectItem mainFormItem = dteProject.ProjectItems.Item("MainForm.cs");
			
			global::EnvDTE.ProjectItems mainFormProjectItems = mainFormItem.ProjectItems;
			
			string[] expectedFiles = new string[] {
				"MainForm.Designer.cs"
			};
			ProjectItemCollectionAssert.AreEqual(expectedFiles, mainFormProjectItems);
		}
		
		[Test]
		public void FileCount_FileProjectItem_ReturnsOne()
		{
			CreateProjectItems();
			msbuildProject.FileName = new FileName(@"d:\projects\MyProject\MyProject.csproj");
			msbuildProject.AddFile(@"program.cs");
			global::EnvDTE.ProjectItem projectItem = projectItems.Item("program.cs");
			
			short count = projectItem.FileCount;
			
			Assert.AreEqual(1, count);
		}
		
		[Test]
		public void Collection_ProjectItemIsFileInProjectRootFolder_ReturnsProjectItemsCollectionForProject()
		{
			CreateProjectItems();
			msbuildProject.FileName = new FileName(@"d:\projects\MyProject\MyProject.csproj");
			msbuildProject.AddFile(@"program.cs");
			global::EnvDTE.ProjectItem projectItem = projectItems.Item("program.cs");
			
			global::EnvDTE.ProjectItems collection = projectItem.Collection;
			
			Assert.AreEqual(dteProject.ProjectItems, collection);
		}
		
		[Test]
		public void Collection_ProjectItemIsFileInSubFolderOfProject_ReturnsProjectItemsCollectionForSubFolder()
		{
			CreateProjectItems();
			msbuildProject.FileName = new FileName(@"d:\projects\MyProject\MyProject.csproj");
			msbuildProject.AddFile(@"src\program.cs");
			global::EnvDTE.ProjectItem srcDirectoryItem = dteProject.ProjectItems.Item("src");
			global::EnvDTE.ProjectItem fileProjectItem = srcDirectoryItem.ProjectItems.Item("program.cs");
			
			global::EnvDTE.ProjectItems collection = fileProjectItem.Collection;
			
			global::EnvDTE.ProjectItem item = collection.Item("program.cs");
			Assert.AreEqual("program.cs", item.Name);
		}
		
		[Test]
		public void Save_FileNameNotPassed_SavesFile()
		{
			CreateProjectItems();
			msbuildProject.FileName = FileName.Create(@"d:\projects\MyProject\MyProject.csproj");
			msbuildProject.AddFile(@"program.cs");
			IViewContent view = OpenFileInSharpDevelop(@"d:\projects\MyProject\program.cs", false);
			global::EnvDTE.ProjectItem item = projectItems.Item("program.cs");
			
			item.Save();
			
			Assert.AreEqual(view, fakeFileService.ViewContentPassedToSaveFile);
		}
		
		[Test]
		public void Save_FileNameNotPassedAndFileNotOpen_FileNotSaved()
		{
			CreateProjectItems();
			msbuildProject.FileName = FileName.Create(@"d:\projects\MyProject\MyProject.csproj");
			msbuildProject.AddFile(@"program.cs");
			global::EnvDTE.ProjectItem item = projectItems.Item("program.cs");
			
			item.Save();
			
			Assert.IsFalse(fakeFileService.IsSaveFileCalled);
		}
	}
}
