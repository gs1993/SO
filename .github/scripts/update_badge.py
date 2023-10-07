import json
import re
import os
from github import Github

def get_mutation_score_from_report(report_path):
    with open(report_path, 'r') as f:
        report_json = json.load(f)
    return report_json['mutationScore']

def update_readme_badge(new_score, readme_path):
    with open(readme_path, 'r') as f:
        readme_contents = f.read()
    
    updated_contents = re.sub(
        r'\!\[Mutation Score\]\(https://img.shields.io/badge/Mutation%20Score-\d+\.?\d*%25-[a-zA-Z]+\.svg\)',
        f'![Mutation Score](https://img.shields.io/badge/Mutation%20Score-{new_score}%25-red.svg)',
        readme_contents
    )

    with open(readme_path, 'w') as f:
        f.write(updated_contents)

if __name__ == '__main__':
    report_path = './path/to/your/stryker/report.json'  # Replace with your Stryker report path
    readme_path = './README.md'  # Replace with your README path if different
    new_score = get_mutation_score_from_report(report_path)
    
    update_readme_badge(new_score, readme_path)
    
    # Commit and push changes to README
    g = Github(os.environ['GITHUB_TOKEN'])
    repo = g.get_repo(os.environ['GITHUB_REPOSITORY'])
    contents = repo.get_contents("README.md", ref="master")
    repo.update_file(contents.path, "Update mutation score badge", updated_contents, contents.sha, branch="master")