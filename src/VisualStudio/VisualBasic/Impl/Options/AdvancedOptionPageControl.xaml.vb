﻿' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports Microsoft.CodeAnalysis
Imports Microsoft.CodeAnalysis.Editing
Imports Microsoft.CodeAnalysis.Editor.Shared.Options
Imports Microsoft.CodeAnalysis.ExtractMethod
Imports Microsoft.CodeAnalysis.Fading
Imports Microsoft.CodeAnalysis.ImplementType
Imports Microsoft.CodeAnalysis.Remote
Imports Microsoft.CodeAnalysis.Structure
Imports Microsoft.CodeAnalysis.SymbolSearch
Imports Microsoft.VisualStudio.LanguageServices.Implementation

Namespace Microsoft.VisualStudio.LanguageServices.VisualBasic.Options
    Friend Class AdvancedOptionPageControl
        Public Sub New(serviceProvider As IServiceProvider)
            MyBase.New(serviceProvider)

            InitializeComponent()

            BindToFullSolutionAnalysisOption(Enable_full_solution_analysis, LanguageNames.VisualBasic)
            BindToOption(Perform_editor_feature_analysis_in_external_process, RemoteFeatureOptions.OutOfProcessAllowed)

            BindToOption(PlaceSystemNamespaceFirst, GenerationOptions.PlaceSystemNamespaceFirst, LanguageNames.VisualBasic)
            BindToOption(SuggestForTypesInReferenceAssemblies, SymbolSearchOptions.SuggestForTypesInReferenceAssemblies, LanguageNames.VisualBasic)
            BindToOption(SuggestForTypesInNuGetPackages, SymbolSearchOptions.SuggestForTypesInNuGetPackages, LanguageNames.VisualBasic)

            BindToOption(EnableOutlining, FeatureOnOffOptions.Outlining, LanguageNames.VisualBasic)
            BindToOption(Show_outlining_for_declaration_level_constructs, BlockStructureOptions.ShowOutliningForDeclarationLevelConstructs, LanguageNames.VisualBasic)
            BindToOption(Show_outlining_for_code_level_constructs, BlockStructureOptions.ShowOutliningForCodeLevelConstructs, LanguageNames.VisualBasic)
            BindToOption(Show_outlining_for_comments_and_preprocessor_regions, BlockStructureOptions.ShowOutliningForCommentsAndPreprocessorRegions, LanguageNames.VisualBasic)
            BindToOption(Collapse_regions_when_collapsing_to_definitions, BlockStructureOptions.CollapseRegionsWhenCollapsingToDefinitions, LanguageNames.VisualBasic)

            BindToOption(Fade_out_unused_imports, FadingOptions.FadeOutUnusedImports, LanguageNames.VisualBasic)

            BindToOption(Show_guides_for_declaration_level_constructs, BlockStructureOptions.ShowBlockStructureGuidesForDeclarationLevelConstructs, LanguageNames.VisualBasic)
            BindToOption(Show_guides_for_code_level_constructs, BlockStructureOptions.ShowBlockStructureGuidesForCodeLevelConstructs, LanguageNames.VisualBasic)

            BindToOption(EnableEndConstruct, FeatureOnOffOptions.EndConstruct, LanguageNames.VisualBasic)
            BindToOption(EnableLineCommit, FeatureOnOffOptions.PrettyListing, LanguageNames.VisualBasic)
            BindToOption(AutomaticInsertionOfInterfaceAndMustOverrideMembers, FeatureOnOffOptions.AutomaticInsertionOfAbstractOrInterfaceMembers, LanguageNames.VisualBasic)
            BindToOption(DisplayLineSeparators, FeatureOnOffOptions.LineSeparator, LanguageNames.VisualBasic)
            BindToOption(EnableHighlightReferences, FeatureOnOffOptions.ReferenceHighlighting, LanguageNames.VisualBasic)
            BindToOption(EnableHighlightKeywords, FeatureOnOffOptions.KeywordHighlighting, LanguageNames.VisualBasic)
            BindToOption(RenameTrackingPreview, FeatureOnOffOptions.RenameTrackingPreview, LanguageNames.VisualBasic)
            BindToOption(GenerateXmlDocCommentsForTripleApostrophes, FeatureOnOffOptions.AutoXmlDocCommentGeneration, LanguageNames.VisualBasic)
            BindToOption(NavigateToObjectBrowser, VisualStudioNavigationOptions.NavigateToObjectBrowser, LanguageNames.VisualBasic)

            BindToOption(DontPutOutOrRefOnStruct, ExtractMethodOptions.DontPutOutOrRefOnStruct, LanguageNames.VisualBasic)
            BindToOption(AllowMovingDeclaration, ExtractMethodOptions.AllowMovingDeclaration, LanguageNames.VisualBasic)

            BindToOption(with_other_members_of_the_same_kind, ImplementTypeOptions.InsertionBehavior, ImplementTypeInsertionBehavior.WithOtherMembersOfTheSameKind, LanguageNames.VisualBasic)
            BindToOption(at_the_end, ImplementTypeOptions.InsertionBehavior, ImplementTypeInsertionBehavior.AtTheEnd, LanguageNames.VisualBasic)

            BindToOption(prefer_throwing_properties, ImplementTypeOptions.PropertyGenerationBehavior, ImplementTypePropertyGenerationBehavior.PreferThrowingProperties, LanguageNames.VisualBasic)
            BindToOption(prefer_auto_properties, ImplementTypeOptions.PropertyGenerationBehavior, ImplementTypePropertyGenerationBehavior.PreferAutoProperties, LanguageNames.VisualBasic)
        End Sub
    End Class
End Namespace
