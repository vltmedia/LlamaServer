using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace LlamaServer.UI
{
    public class MultilineTextEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal; // Opens a modal editor
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (editorService == null)
                return value;

            using (Form form = new Form())
            using (TextBox textBox = new TextBox())
            {
                form.Text = "Edit Text";
                form.Width = 400;
                form.Height = 300;
                form.StartPosition = FormStartPosition.CenterParent;

                textBox.Multiline = true;
                textBox.ScrollBars = ScrollBars.Both;
                textBox.Dock = DockStyle.Fill;
                textBox.Text = value as string;

                Button okButton = new Button { Text = "OK", Dock = DockStyle.Bottom };
                okButton.Click += (sender, e) => form.DialogResult = DialogResult.OK;

                form.Controls.Add(textBox);
                form.Controls.Add(okButton);

                if (editorService.ShowDialog(form) == DialogResult.OK)
                {
                    return textBox.Text;
                }
            }
            return value;
        }
    }
}
