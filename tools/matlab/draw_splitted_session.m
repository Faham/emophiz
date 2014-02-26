
%-------------------------------------------------------------------------------

addpath('D:/projects/emophiz/tools/matlab');

root = 'D:/projects/emophiz/data/logs/experiment/splitted';

%-------------------------------------------------------------------------------

%1 time_millisecond
%2 arousal
%3 player_speed
%4 zombie_speed
%5 fog_start_dist
%6 fog_end_dist
%7 current_round
%8 zombie_threshold
%9 zombie_increase_power
%10 max_zombie_alive
%11 number_of_alive_zombies
%12 number_of_killed_zombies
%13 grenade_regen_delay
%14 medic_regen_delay
%15 calibrating
%16 adaptation_condition

%-------------------------------------------------------------------------------

file_id = {
    10
    12
    13
    14
    15
    16
    17
    18
    19
    20
    21
    22
    23
    24
    25
};

phase_offset = 500000;
signal_offset = 100;
means = zeros(26, 4);
ranges = zeros(26, 4);
killed_zombies = zeros(26, 4);
max_round = zeros(26, 4);
adaptation_value = zeros(26, 4);

for j=1:4
    mn = 0;
    for i=1:length(file_id)
        gsr_filename            = sprintf('%s/%d_%s_%d.csv', root, file_id{i}, 'gsr', j);
        metrics_filename        = sprintf('%s/%d_%s_%d.csv', root, file_id{i}, 'metrics', j);

        %gsr_filename            = sprintf('%s/%d_%s.csv', root, file_id{i}, 'gsr');
        %metrics_filename        = sprintf('%s/%d_%s.csv', root, file_id{i}, 'metrics');

        [gsr_h, gsr_v]          = readCSV(gsr_filename, 3);
        [metrics_h, metrics_v]  = readCSV(metrics_filename, 16);

        gsr_signal_clamped = gsr_v{2} - gsr_v{2}(1);
        gsr_time_adjusted = gsr_v{1} - gsr_v{1}(1) + phase_offset * j;

        means(file_id{i}, j) = mean(gsr_signal_clamped) * 1000;
        ranges(file_id{i}, j) = (max(gsr_signal_clamped) - min(gsr_signal_clamped)) * 1000;
        killed_zombies(file_id{i}, j) = max(metrics_v{12});
        max_round(file_id{i}, j) = max(metrics_v{7});
        if j == 2
            adaptation_value(file_id{i}, j) = mean(abs(metrics_v{j+1} - 1) / 1.35) * 100;
        elseif j == 3
            adaptation_value(file_id{i}, j) = mean(abs(metrics_v{j+1} - 1) / 2.57) * 100;
        elseif j == 4
            adaptation_value(file_id{i}, j) = mean(abs(metrics_v{j+1} - 300) / 380) * 100;
        else
            adaptation_value(file_id{i}, j) = 0;
        end

        %plot(gsr_time_adjusted, smooth(gsr_v{1}, gsr_signal_clamped * 1000 + signal_offset * i, 0.1, 'loess'), 'g-'); hold on;

        %plot(gsr_v{1}, gsr_v{2} * 1000 + 700, 'b-'); hold on; % gsr
        %plot(metrics_v{1}, metrics_v{15} * 100 + 25, 'k-'); hold on; % calibration
        %plot(metrics_v{1}, metrics_v{16} * 20 + 50, 'c-'); hold on; % adaptation
        %plot(metrics_v{1}, (metrics_v{3}) * 100 / 2.3, 'y-'); %hold on; % player speed
        %plot(metrics_v{1}, (metrics_v{4} - 1 ) * 100 / 3, 'g-'); %hold on; % zombie speed
        %plot(metrics_v{1}, (metrics_v{5} - 70) * 100 / 380, 'm-'); %hold on; % fog distant
        %legend('GSR Data', 'Calibration', 'Condition', 'Player Speed', 'Zombie Speed', 'Fog Distant', 'Location','NW')

        xlabel('time');
        ylabel('signal');
        %grid on;
    end

end

bar(means, 0.4);
bar(ranges, 0.2);


%-------------------------------------------------------------------------------
