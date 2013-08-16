function [headers, value] = readCSV(filename, rec_length)
	headers_ptrn = '';
	for i = 1:rec_length
		if ~isempty(headers_ptrn)
			headers_ptrn = [headers_ptrn, ' '];
		end
		headers_ptrn = [headers_ptrn, '%s'];
	end
	
	value_ptrn = strrep(headers_ptrn, 's', 'f');
	
	fid = fopen(filename);
	headers = textscan(fid, headers_ptrn, 1, 'delimiter', ',');
	value = textscan(fid, value_ptrn,'delimiter',',');
	fclose(fid);
end
