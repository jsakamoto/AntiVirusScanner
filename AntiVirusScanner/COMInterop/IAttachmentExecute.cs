using System;
using System.Runtime.InteropServices;

namespace AntiVirus.COMInterop
{
    /// <summary>
    /// Provides a set of flags to be used with IAttachmentExecute::Prompt to indicate the type of prompt UI to display.
    /// </summary>
    [ComVisible(false)]
    public enum ATTACHMENT_PROMPT
    {
        /// <summary>Do not use.</summary>
        ATTACHMENT_PROMPT_NONE,
        /// <summary>Displays a prompt asking whether the user would like to save the attachment.</summary>
        ATTACHMENT_PROMPT_SAVE,
        /// <summary>Displays a prompt asking whether the user would like to execute the attachment.</summary>
        ATTACHMENT_PROMPT_EXEC,
        /// <summary>Displays a prompt giving the user a choice of executing or saving the attachment.</summary>
        ATTACHMENT_PROMPT_EXEC_OR_SAVE
    }

    /// <summary>
    /// Provides a set of flags to be used with IAttachmentExecute::Prompt to indicate the action to be performed upon user confirmation.
    /// </summary>
    [ComVisible(false)]
    public enum ATTACHMENT_ACTION
    {
        /// <summary>Cancel</summary>
        ATTACHMENT_ACTION_CANCEL,
        /// <summary>Save</summary>
        ATTACHMENT_ACTION_SAVE,
        /// <summary>Execute</summary>
        ATTACHMENT_ACTION_EXEC
    }

    /// <summary>
    /// Exposes methods that work with client applications to present a user environment that provides safe download and exchange of files through email and messaging attachments.
    /// </summary>
    [ComImport, Guid("73db1241-1e85-4581-8e4f-a81e1d0f8c57"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), ComVisible(false)]
    public interface IAttachmentExecute
    {
        /// <summary>
        /// Specifies and stores the title of the prompt window.
        /// </summary>
        /// <param name="pszTitle">A string that contains the title text.</param>
        void SetClientTitle(string pszTitle);

        /// <summary>
        /// Specifies and stores the GUID for the client.
        /// </summary>
        /// <param name="guid">The GUID that represents the client.</param>
        void SetClientGuid(ref Guid guid);

        /// <summary>
        /// A string that contains the file name.
        /// </summary>
        /// <param name="pszLocalPath">A string that contains the local path where the attachment file is to be stored.</param>
        void SetLocalPath(string pszLocalPath);

        /// <summary>
        /// Specifies and stores the proposed name of the file.
        /// </summary>
        /// <param name="pszFileName">A string that contains the file name.</param>
        void SetFileName(string pszFileName);

        /// <summary>
        /// Sets an alternate path or URL for the source of a file transfer.
        /// </summary>
        /// <param name="pszSource">A string containing the path or URL to use as the source.</param>
        void SetSource(string pszSource);

        /// <summary>
        /// Sets the security zone associated with the attachment file based on the referring file.
        /// </summary>
        /// <param name="pszReferrer">A string containing the path of the referring file.</param>
        void SetReferrer(string pszReferrer);

        /// <summary>
        /// Provides a Boolean test that can be used to make decisions based on the attachment's execution policy.
        /// </summary>
        /// <returns>S_OK meaning Enable, S_FALSE meaning Prompt, Any other failure code meaning Disable.</returns>
        [PreserveSig]
        uint CheckPolicy();

        /// <summary>
        /// Presents a prompt UI to the user.
        /// </summary>
        /// <param name="hwnd">A handle to the parent window.</param>
        /// <param name="prompt">A member of the ATTACHMENT_PROMPT enumeration that indicates what type of prompt UI to display to the user.</param>
        /// <returns>A member of the ATTACHMENT_ACTION enumeration that indicates the action to be performed upon user confirmation.</returns>
        ATTACHMENT_ACTION Prompt(IntPtr hwnd, ATTACHMENT_PROMPT prompt);

        /// <summary>
        /// A member of the ATTACHMENT_ACTION enumeration that indicates the action to be performed upon user confirmation.
        /// </summary>
        /// <returns>If this method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [PreserveSig]
        uint Save();

        /// <summary>
        /// Executes an action on an attachment.
        /// </summary>
        /// <param name="hwnd">The handle of the parent window.</param>
        /// <param name="pszVerb">A string that contains a verb specifying the action to be performed on the file. See the lpOperation parameter in ShellExecute for valid strings. This value can be NULL.</param>
        /// <param name="phProcess">A pointer to a handle to the source process, used for synchronous operation. This value can be NULL.</param>
        void Execute(IntPtr hwnd, string pszVerb, ref IntPtr phProcess);

        /// <summary>
        /// Presents the user with explanatory error UI if the save action fails.
        /// </summary>
        /// <param name="hwnd">The handle of the parent window.</param>
        void SaveWithUI(IntPtr hwnd);

        /// <summary>
        /// Removes any stored state that is based on the client's GUID. An example might be a setting based on a checked box that indicates a prompt should not be displayed again for a particular file type.
        /// </summary>
        void ClearClientState();
    }
}
