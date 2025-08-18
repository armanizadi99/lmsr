namespace Lmsr.Application.ViewModels;

public record  WordViewModel(int Id, string Term, List<WordDefinitionViewModel> Words, int CourseId);