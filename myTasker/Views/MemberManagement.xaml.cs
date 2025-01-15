using Domain;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace myTasker.Views
{
    public partial class MemberManagement : UserControl
    {
        private readonly MemberService _memberService;
        private Member _editingMember; // Référence au membre en cours de modification

        public MemberManagement(MemberService memberService)
        {
            InitializeComponent();
            _memberService = memberService;
            LoadMembers();
        }

        private async void LoadMembers()
        {
            var members = await _memberService.GetAllMembersAsync();
            MembersListView.ItemsSource = new ObservableCollection<Member>(members);
        }

        private async void SaveMemberButton_Click(object sender, RoutedEventArgs e)
        {
            // Valider les entrées
            if (string.IsNullOrWhiteSpace(MemberNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(MemberEmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(MemberRoleTextBox.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            if (_editingMember != null)
            {
                // Mettre à jour le membre existant
                _editingMember.Name = MemberNameTextBox.Text;
                _editingMember.Email = MemberEmailTextBox.Text;
                _editingMember.Role = MemberRoleTextBox.Text;

                await _memberService.UpdateMemberAsync(_editingMember);
                MessageBox.Show("Membre mis à jour !");
                _editingMember = null; // Réinitialiser le mode d'édition
            }
            else
            {
                // Ajouter un nouveau membre
                var newMember = new Member
                {
                    Name = MemberNameTextBox.Text,
                    Email = MemberEmailTextBox.Text,
                    Role = MemberRoleTextBox.Text
                };

                await _memberService.AddMemberAsync(newMember);
                MessageBox.Show("Membre ajouté !");
            }

            // Rafraîchir la liste et réinitialiser les champs
            LoadMembers();
            ResetForm();
        }

        private void ResetForm()
        {
            MemberNameTextBox.Text = string.Empty;
            MemberEmailTextBox.Text = string.Empty;
            MemberRoleTextBox.Text = string.Empty;
            _editingMember = null;
        }

        private void EditMemberButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button editButton && editButton.Tag is Member member)
            {
                // Charger les informations dans les champs de saisie
                _editingMember = member;
                MemberNameTextBox.Text = member.Name;
                MemberEmailTextBox.Text = member.Email;
                MemberRoleTextBox.Text = member.Role;
            }
        }

        private async void DeleteMemberButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button deleteButton && deleteButton.Tag is Member member)
            {
                var result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer {member.Name} ?",
                                              "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    await _memberService.DeleteMemberAsync(member.Id);
                    LoadMembers();
                }
            }
        }

        private void MembersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Optionnel : Réinitialiser le formulaire si aucun membre n'est sélectionné
            if (MembersListView.SelectedItem == null)
            {
                ResetForm();
            }
        }
        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                RaisePropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
