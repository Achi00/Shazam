using FluentValidation;
using Shazam.Application.DTOs.Song;

namespace Shazam.Application.Validation.Song
{
    public class AddSongRequestValidator : AbstractValidator<AddSongRequest>
    {
        public AddSongRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty();

            RuleFor(x => x.YoutubeUrl)
                .Must(ValidateUrl);
        }

        private bool ValidateUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}
