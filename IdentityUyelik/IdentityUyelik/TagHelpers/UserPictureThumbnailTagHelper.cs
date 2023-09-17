using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityUyelik.TagHelpers
{
    public class UserPictureThumbnailTagHelper : TagHelper
    {

        public string PictureUrl { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";

            if (PictureUrl == null)
            {
                output.Attributes.SetAttribute("src", "./UserPicture/Default_User _Picture.png");
            }
            else
            {
                output.Attributes.SetAttribute("src", $"./UserPicture/{PictureUrl}");
            }


            base.Process(context, output);
        }

    }
}
