import glob
import json

# Search for mutation-report.json in directories under a specific path
report_files = glob.glob('/home/runner/work/SO/SO/SO/Tests/UnitTests/StrykerOutput/*/reports/mutation-report.json')

if report_files:
    report_file = report_files[0]
    
    with open(report_file, 'r') as f:
        data = json.load(f)
        
    mutation_score = data.get('mutationScore', None)
    if mutation_score is not None:
        print(f'Mutation score is {mutation_score}')
else:
    print("No report files found.")