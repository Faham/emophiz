function allData = ttltestNEW(TTLLive);
% TTLAPI Matlab sample code
% View streaming data input from channels C and D
%
%   Call mInitTTL before running ttltestNEW and mDestroyTTL afterwards
%
%
% Programmer: Eamon Egan
% Copyright 2002 Thought Technology Ltd.
%
% May 2005, Marc Saab
% added:  uV units and plotting of streaming data (on channels C and D)
%           

% some "defines"
% from TTLAPI.H
TTLAPI_OCCMD_AUTODETECT = 1;
TTLAPI_UT_COUNT         = 9;
TTLAPI_UT_SENSUVOLT     = 13;
TRUE = 1;
FALSE = 0;

PollingInterval = 1000  % 1000 mS = 1 second, the maximum interval allowed between data reads

% hide ActiveX window and create new window for plotting
hxwin = gcf;
set(hxwin, 'Visible', 'off');
hmain = figure;

% Connect to encoder using the autodetection feature
try
    % The last 2 parameters are output parameter pointers, which don't work in Matlab
    invoke(TTLLive, 'OpenConnections', TTLAPI_OCCMD_AUTODETECT, PollingInterval,0,0);
catch
    disp('Failed in OpenConnections');
    delete(TTLLive); close all; return;
end 

disp('Encoder count is: '); 
disp(TTLLive.EncoderCount);

% Automatically create basic TTLAPI channels from encoder physical channels
try
  invoke(TTLLive, 'AutoSetupChannels');
catch
  disp('Failed in AutoSetupChannels');
  delete(TTLLive); close all; return;
end 

disp('Channel count is');
nChans = TTLLive.ChannelCount;
disp(nChans);

% Create chanHND array (an array of channel handles, which must be used 
% to access channels).
chanHND = [];
hChannelHND = invoke(TTLLive,'GetFirstChannelHND');
while hChannelHND ~= -1
    % Save the channel handles
    chanHND = [chanHND,hChannelHND];

    % Get next TTLAPI created channel
    hChannelHND = invoke(TTLLive,'GetNextChannelHND');
end
chanHND = chanHND(3:4);   % we are only interested in channels C and D

% Display the physical channels
disp('Physical channels (zero-based indeces):');
physChan = [];
for cH = chanHND
    physChan = [physChan, get(TTLLive,'ChannelPhysicalIndex',cH)];
end
disp(physChan);

% Set up some channel properties
for cH = chanHND
    IsConnected = get(TTLLive, 'SensorConnected', cH);
    if(IsConnected)
        set(TTLLive,'ChannelActive', cH, TRUE); % set all channels active
        sID = get(TTLLive,'SensorID',cH);
        set(TTLLive,'SensorType',cH,sID);
        %set(TTLLive,'UnitType',cH,TTLAPI_UT_SENSUVOLT); 
    else
        disp('Error: Sensor connection absent')
        delete(TTLLive); close all; return;
    end
end

% Now that all channels are set up, let's start them
try
  invoke(TTLLive,'StartChannels');
catch
  disp('Failed in StartChannels');
  delete(TTLLive); close all; return;
end 

if nChans == 0
    disp('No channels initialized')
    delete(TTLLive); close all; return;
end

alltimeFactor = 50; % number of epochs to plot
PausePeriod = 0.0625;   % loop pause interval (necessary for streaming display)

% sampling frequency (channels C-D, ProComp protocol, Fs = 256 Hz)
Fs = 256;  
P = 1/Fs;
% epoch sizes
requiredSamples = Fs * 0.500; % current epoch window
updateSamples = Fs * PausePeriod; % incoming data block

% initialize data structures (array dim is channel x sample)
newData = zeros(2,requiredSamples);    
allData = zeros(2,requiredSamples*alltimeFactor);
sA = get(TTLLive,'SamplesAvailable',chanHND(1)); % check channel C
while sA < requiredSamples
    sA = get(TTLLive,'SamplesAvailable',chanHND(1)); % check channel C
end
for i = 1:2
    newData(i,:) = invoke(TTLLive,'ReadChannelDataVT',chanHND(i),requiredSamples);
    allData(i,(alltimeFactor-1)*requiredSamples+1:alltimeFactor*requiredSamples) = newData(i,:);
end

% create subplots [left, bottom, width, height]
f1 = axes('position', [0.05, 0.65, 0.425, 0.3]);
f2 = axes('position', [0.525, 0.65, 0.425, 0.3]);
f3 = axes('position', [0.05, 0.325, 0.9, 0.225]);
f4 = axes('position', [0.05, 0.05, 0.9, 0.225]);

% x-axes timescales
t = -0.5+P:P:0;
t2 = -25+P:P:0;

% begin data stream
% run for 2 minutes or until ESC key is pressed
tic;
while (toc < 120)

    pause(1e-10);
    
    sA = get(TTLLive,'SamplesAvailable',chanHND(1));
    while sA < updateSamples
        sA = get(TTLLive,'SamplesAvailable',chanHND(1)); 
    end
    for i = 1:2
        temp = invoke(TTLLive,'ReadChannelDataVT',chanHND(i), updateSamples);
        newData(i,:) = [newData(1,updateSamples+1:requiredSamples) temp];
        allData(i,:) = [allData(1,updateSamples+1:alltimeFactor*requiredSamples) temp];
    end
    
    % plot data
    subplot(f1)
    plot(t,newData(1,:));
    set(gca,'XLim',[-0.5 0]);
    title('Channel C - current epoch')
    
    subplot(f2);
    plot(t,newData(2,:));
    set(gca,'XLim',[-0.5 0]);
    title('Channel D - current epoch')
    
    subplot(f3)
    plot(t2,allData(1,:));
    title('Channel C')

    subplot(f4)
    plot(t2,allData(2,:));
    title('Channel D')
          
    % react to escape keypress
    Key = get(gcf,'currentkey');
    if strcmp(Key, 'escape')
        disp('processing terminated by user...')
        break;
    end

end

return

